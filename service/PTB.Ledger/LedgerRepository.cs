using System.Linq;
using PTB.Core.Base;
using PTB.Core.Exceptions;
using PTB.Core.Statements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using PTB.Core.Logging;
using PTB.Core;
using PTB.Core.TitleRegex;

namespace PTB.Ledger
{
    public class LedgerRepository : BaseFileRepository
    {
        public LedgerRepository(IPTBLogger logger, BaseParser parser, FolderSchema schema, BasePTBFile file) : base(logger, parser, schema, file)
        {
            _logger.SetContext(nameof(LedgerRepository));
        }

        /*
        public void ImportToDefaultLedger(string importPath, IStatementParser parser, bool append = false)
        {
            string ledgerPath = _fileManager.GetDefaultLedgerFilePath();

            using (var writer = new StreamWriter(ledgerPath, append))
            {
                string line;
                using (StreamReader reader = new StreamReader(importPath))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        StatementParseResponse response = parser.ParseLine(line, _schema);

                        if (response.Success)
                        {
                            writer.WriteLine(response.Result);
                        }
                    }
                }
            }
        } */

        public BaseReadResponse ReadDefaultLedgerEntries(int startIndex, int ledgerCount)
        {
            var response = BaseReadResponse.Default;
            response = base.Read(startIndex, ledgerCount);
            return response;
        }

        public BaseUpdateResponse UpdateDefaultLedgerEntry(int index, PTBRow row)
        {
            var response = BaseUpdateResponse.Default;
            base.Update(index, row);
            return response;
        }

        public string GetRegexMatch(string regex) => String.Concat(".*", regex.TrimStart(), "*");

        public string GetColumnValueByName(string name, PTBRow row)
        {
            return row.Columns.First(column => column.ColumnName.Equals(name, StringComparison.OrdinalIgnoreCase))?.ColumnValue;
        }
        public PTBRow SetColumnValueByName(string name, string value, PTBRow row)
        {
            row.Columns.ForEach(column => {
                if (column.ColumnName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    column.ColumnValue = value;
                }
            });
            return row;
        }

        public bool IsLedgerLocked(PTBRow row) => GetColumnValueByName("locked", row) == "1";

        public void CategorizeDefaultLedger(IEnumerable<TitleRegex.TitleRegex> titleRegices)
        {
            using (var stream = new FileStream(_file.FullName, FileMode.Open, FileAccess.ReadWrite))
            {
                int bufferLength = _schema.LineSize + Environment.NewLine.Length;
                int lineIndex = _schema.LineSize - 1;
                var buffer = new byte[bufferLength];
                int bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string line = _encoding.GetString(buffer);

                    // the byte order mark (byteID 239) is added by some utf-8 compatible text editors. Can remove using Vim by :set nobomb; wq
                    if (HasByteOrderMark(buffer))
                    {
                        throw new ParseException("The default ledger has a byte order mark added by a utf-8 compatible editor. Please remove from the text file to continue.");
                    }

                    // editing text files on a Unix-based can convert the line endings. Fix by running the unixtodos command
                    if (HasUnixNewLine(buffer))
                    {
                        throw new ParseException("The default ledger has Unix new lines instead of Windows new lines. Please convert to windows new lines to continue");
                    }

                    var parseResponse = _parser.ParseLine(line, bytesRead);

                    if (!parseResponse.Success)
                    {
                        long lineNumber = GetLineNumber(stream.Position, _schema.LineSize);
                        throw new ParseException($"Review the default ledger for data corruption at line {lineNumber}. Message is: {parseResponse.Message}");
                    }

                    // skips ledgers that have been locked by user modification
                    if (IsLedgerLocked(parseResponse.Row))
                    {
                        continue;
                    }

                    foreach (var titleRegex in titleRegices)
                    {
                        string match = GetRegexMatch(titleRegex.Regex);

                        string title = GetColumnValueByName("title", parseResponse.Row);
                        bool isMatch = Regex.IsMatch(title, match, RegexOptions.IgnoreCase);

                        if (isMatch)
                        {
                            SetColumnValueByName("subcategory", titleRegex.Subcategory, parseResponse.Row);
                            SetColumnValueByName("subject", titleRegex.Subject, parseResponse.Row);
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