using PTB.Core.Exceptions;
using PTB.Core.Statements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace PTB.Core.Ledger
{
    public class LedgerRepository : BaseFileRepository
    {
        private string _Folder = "Ledgers";
        private LedgerParser _parser;
        private Encoding _encoding = Encoding.ASCII;

        public LedgerRepository(FileManager fileManager) : base(fileManager)
        {
            _parser = new LedgerParser(_schema.Ledger);
        }

        public void ImportToDefaultLedger(string path, IStatementParser parser, bool append = false)
        {
            FileInfo ledgerFile = _fileManager.GetDefaultLedgerFile();

            using (var writer = new StreamWriter(ledgerFile.FullName, append))
            {
                string line;
                using (StreamReader reader = new StreamReader(path))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        StatementParseResponse response = parser.ParseLine(line, _schema.Ledger);

                        if (response.Success)
                        {
                            writer.WriteLine(response.Result);
                        }
                    }
                }
            }
        }

        public bool IndexStartsAtCorrectByte(int startIndex)
        {
            if (startIndex == 0) { return true; }

            if (startIndex > 0)
            {
                // subtracts the first byte of the line to the start Index (e.g. a 117 byte line will start the next line on 118)
                return (startIndex % (_schema.Ledger.LineSize + Environment.NewLine.Length)) == 0;
            }
            return false;
        }

        public byte[] GetLedgerBuffer() => new byte[_schema.Ledger.LineSize + Environment.NewLine.Length];

        public void SetBufferStartIndex(FileStream stream, int startIndex) => stream.Seek(startIndex, SeekOrigin.Begin);

        public List<Ledger> ReadDefaultLedgerEntries(int startIndex, int ledgerCount)
        {
            if (!IndexStartsAtCorrectByte(startIndex))
            {
                throw new Exception($"The start index {startIndex} does not match the index of any ledger line. It should be divisible by {_schema.Ledger.LineSize}");
            }

            var ledgerEntries = new List<Ledger>();
            FileInfo ledgerFile = _fileManager.GetDefaultLedgerFile();

            using (var stream = new FileStream(ledgerFile.FullName, FileMode.Open, FileAccess.Read))
            {
                int byteIndex = startIndex;
                int bytesRead = 0;
                byte[] buffer = GetLedgerBuffer();
                SetBufferStartIndex(stream, startIndex);

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0 && ledgerCount > 0)
                {
                    string line = _encoding.GetString(buffer);
                    StringToLedgerResponse current = _parser.ParseLine(line, byteIndex);

                    if (!current.Success)
                    {
                        long lineNumber = GetLineNumber(stream.Position, _schema.Ledger.LineSize);
                        throw new ParseException($"Review the default ledger for data corruption at line {lineNumber}. Message is: {current.Message}");
                    }

                    ledgerEntries.Add(current.Result);
                    ledgerCount--;
                    byteIndex += bytesRead;
                }
            }

            return ledgerEntries;
        }

        public LedgerUpdateResponse UpdateDefaultLedgerEntry(Ledger ledgerToUpdate)
        {
            var response = LedgerUpdateResponse.Default;

            if (!IndexStartsAtCorrectByte(ledgerToUpdate.Index))
            {
                response.Success = false;
                response.Message = $"Could not update ledger with these values. The start index {ledgerToUpdate} does not match the index of any ledger line. It should be divisible by {_schema.Ledger.LineSize}";
                return response;
            }

            FileInfo ledgerFile = _fileManager.GetDefaultLedgerFile();
            string line = _parser.ParseLedger(ledgerToUpdate);

            using (var stream = new FileStream(ledgerFile.FullName, FileMode.Open, FileAccess.Write))
            {
                byte[] buffer = _encoding.GetBytes(line);
                SetBufferStartIndex(stream, ledgerToUpdate.Index);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }

            return response;
        }

        public string GetRegexMatch(string regex) => String.Concat(".*", regex.TrimStart(), "*");

        public bool IsLedgerLocked(char locked) => locked == '1';

        public void CategorizeDefaultLedger(IEnumerable<TitleRegex.TitleRegex> titleRegices)
        {
            FileInfo ledgerFile = _fileManager.GetDefaultLedgerFile();
            using (var stream = new FileStream(ledgerFile.FullName, FileMode.Open, FileAccess.ReadWrite))
            {
                int bufferLength = _schema.Ledger.LineSize + Environment.NewLine.Length;
                int lineIndex = _schema.Ledger.LineSize - 1;
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

                    StringToLedgerResponse current = _parser.ParseLine(line, bytesRead);

                    if (!current.Success)
                    {
                        long lineNumber = GetLineNumber(stream.Position, _schema.Ledger.LineSize);
                        throw new ParseException($"Review the default ledger for data corruption at line {lineNumber}. Message is: {current.Message}");
                    }

                    Ledger ledger = current.Result;

                    // skips ledgers that have been locked by user modification
                    if (IsLedgerLocked(ledger.Locked))
                    {
                        continue;
                    }

                    foreach (var titleRegex in titleRegices)
                    {
                        string match = GetRegexMatch(titleRegex.Regex);
                        bool isMatch = Regex.IsMatch(ledger.Title, match, RegexOptions.IgnoreCase);

                        if (isMatch)
                        {
                            ledger.Subcategory = titleRegex.Subcategory;
                            ledger.Subject = titleRegex.Subject;
                            string newLine = _parser.ParseLedger(ledger);
                            newLine += Environment.NewLine;
                            byte[] newBuffer = _encoding.GetBytes(newLine);

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