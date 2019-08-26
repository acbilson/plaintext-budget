using PTB.Core.Base;
using PTB.Core.Exceptions;
using PTB.Core.Files;
using PTB.Core.Logging;
using PTB.Core.Statements;
using PTB.Files.FolderAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PTB.Files.Ledger
{
    public class LedgerService : BaseFileService
    {
        public LedgerService(IPTBLogger logger, LedgerFileParser parser, LedgerSchema schema) : base(logger, parser, schema)
        {
            _logger.SetContext(nameof(LedgerService));
        }

        public void Import(LedgerFile file, string importPath, IStatementParser parser, bool append = false)
        {
            using (var writer = new StreamWriter(file.FullPath, append))
            {
                string line;
                using (StreamReader reader = new StreamReader(importPath))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        StatementParseResponse response = parser.ParseLine(line);

                        if (response.Success)
                        {
                            writer.WriteLine(response.Result);
                        }
                        else
                        {
                            _logger.LogWarning($"Import failed with the following message: {response.Message}");
                        }
                    }
                }
            }
        }

        public BaseReadResponse Read(LedgerFile file, int startIndex, int ledgerCount)
        {
            var response = BaseReadResponse.Default;
            response = base.Read(file, startIndex, ledgerCount);
            return response;
        }

        public BaseUpdateResponse Update(LedgerFile file, int index, PTBRow row)
        {
            var response = BaseUpdateResponse.Default;
            base.Update(file, index, row);
            return response;
        }

        public string GetRegexMatch(string regex) => String.Concat(".*", regex.TrimStart(), "*");

        public string GetColumnValueByName(string name, PTBRow row)
        {
            return row.Columns.First(column => column.ColumnName.Equals(name, StringComparison.OrdinalIgnoreCase))?.ColumnValue;
        }

        public bool IsLedgerLocked(PTBRow row) => row["locked"] == "1";

        public void Categorize(LedgerFile file, List<PTBRow> titleRegices)
        {
            using (var stream = new FileStream(file.FullPath, FileMode.Open, System.IO.FileAccess.ReadWrite))
            {
                int bufferLength = _schema.LineSize + Environment.NewLine.Length;
                int lineIndex = _schema.LineSize - 1;
                var buffer = new byte[bufferLength];
                int bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string line = _encoding.GetString(buffer);

                    if (IsFirstLine(bytesRead, buffer.Length))
                    {
                        ValidateBuffer(buffer, file.FileName);
                    }

                    var parseResponse = _parser.ParseLine(line, bytesRead);

                    if (!parseResponse.Success)
                    {
                        long lineNumber = GetLineNumber(stream.Position, _schema.LineSize);
                        string message = $"Review file {file.FileName} for data corruption at line {lineNumber}. Message is: {parseResponse.Message}";
                        throw new ParseException(message);
                    }

                    // skips ledgers that have been locked by user modification
                    if (IsLedgerLocked(parseResponse.Row))
                    {
                        continue;
                    }

                    foreach (var titleRegex in titleRegices)
                    {
                        string match = GetRegexMatch(titleRegex["regex"]);

                        string title = parseResponse.Row["title"];
                        bool isMatch = Regex.IsMatch(title, match, RegexOptions.IgnoreCase);

                        if (isMatch)
                        {
                            parseResponse.Row["subcategory"] = titleRegex["subcategory"];
                            parseResponse.Row["subject"] = titleRegex["subcategory"];
                            var newParseResponse = _parser.ParseRow(parseResponse.Row);
                            newParseResponse.Line += Environment.NewLine;
                            byte[] newBuffer = _encoding.GetBytes(newParseResponse.Line);

                            // returns the stream to the beginning of the buffer (moves forward at Read())
                            stream.Seek(-bytesRead, SeekOrigin.Current);
                            stream.Write(newBuffer, 0, newBuffer.Length);

                            // required for file to be updated.
                            stream.Flush();

                            // will only match first occurence, not overwrite with second, third, etc.
                            continue;
                        }
                    }
                }
            }
        }
    }
}