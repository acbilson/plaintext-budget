using PTB.File.Exceptions;
using PTB.File.Statements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace PTB.File.Ledger
{
    public class LedgerRepository : BaseFileRepository
    {
        private string _Folder = "Ledgers";
        private LedgerParser _parser;
        private Encoding _encoding = Encoding.ASCII;

        public LedgerRepository(PTBSettings settings, PTBSchema schema) : base(settings, schema)
        {
            _parser = new LedgerParser(_schema.Ledger);
        }

        public void ImportToDefaultLedger(string path, IStatementParser parser, bool append = false)
        {
            string ledgerPath = base.GetDefaultPath(_Folder, _schema.Ledger.GetDefaultName());

            using (var writer = new StreamWriter(ledgerPath, append))
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

            if (startIndex > 0) { 
                // subtracts the first byte of the line to the start Index (e.g. a 117 byte line will start the next line on 118)
                return (startIndex - 1) % _schema.Ledger.Size == 0;
            }
            return false;
        }

        public byte[] GetLedgerBuffer() => new byte[_schema.Ledger.Size + Environment.NewLine.Length];

        public void SetBufferStartIndex(FileStream stream, int startIndex) => stream.Seek(startIndex, SeekOrigin.Begin);

        public long GetLineIndex(long streamPosition) => streamPosition / _schema.Ledger.Size;

        public List<Ledger> ReadDefaultLedgerEntries(int startIndex, int ledgerCount)
        {
            if (!IndexStartsAtCorrectByte(startIndex))
            {
                throw new Exception($"The start index {startIndex} does not match the index of any ledger line. It should be divisible by {_schema.Ledger.Size}"); 
            }

            var ledgerEntries = new List<Ledger>();
            string ledgerPath = base.GetDefaultPath(_Folder, _schema.Ledger.GetDefaultName());

            using (var stream = new FileStream(ledgerPath, FileMode.Open, FileAccess.Read))
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
                        long lineNumber = GetLineIndex(stream.Position);
                        throw new ParseException($"Review the default ledger for data corruption at line {lineNumber}. Message is: {current.Message}");
                    }

                    ledgerEntries.Add(current.Result);
                    ledgerCount--;
                    byteIndex += bytesRead;
                }
            }

            return ledgerEntries;
        }

        public string GetRegexMatch(string regex) => String.Concat(".*", regex.TrimStart(), "*");

        public bool HasByteOrderMark(byte[] buffer) => buffer[0] == 239;

        // Windows new line has both byte 13 (\n) and byte 10 (\r). Unix only has byte 13 (\n), so it will not contain byte 10 (\r)
        public bool HasUnixNewLine(byte[] buffer) => buffer.Any((b) => b == 10) == false;

        public void CategorizeDefaultLedger(IEnumerable<TitleRegex.TitleRegex> titleRegices)
        {
            string ledgerPath = base.GetDefaultPath(_Folder, _schema.Ledger.GetDefaultName());
            using (var stream = new FileStream(ledgerPath, FileMode.Open, FileAccess.ReadWrite))
            {
                int bufferLength = _schema.Ledger.Size + Environment.NewLine.Length;
                int lineIndex = _schema.Ledger.Size - 1;
                var buffer = new byte[bufferLength];
                int bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
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


                    string line = _encoding.GetString(buffer);
                    StringToLedgerResponse current = _parser.ParseLine(line, bytesRead);

                    if (!current.Success)
                    {
                        long lineNumber = GetLineIndex(stream.Position);
                        throw new ParseException($"Review the default ledger for data corruption at line {lineNumber}. Message is: {current.Message}");
                    }

                    Ledger ledger = current.Result;

                    foreach (var titleRegex in titleRegices)
                    {
                        string match = GetRegexMatch(titleRegex.Regex);
                        bool isMatch = Regex.IsMatch(ledger.Title, match, RegexOptions.IgnoreCase);

                        if (isMatch)
                        {
                            ledger.Subcategory = titleRegex.Subcategory;
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