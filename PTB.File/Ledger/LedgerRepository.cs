using PTB.File.Statements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace PTB.File.Ledger
{
    public class LedgerRepository : BaseFileRepository
    {
        private string _Folder = "Ledgers";
        private LedgerParser _parser;

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
                        ParseResponse response = parser.ParseLine(line, _schema.Ledger);

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
                    string line = Encoding.ASCII.GetString(buffer);
                    Ledger current = _parser.ParseLine(line, byteIndex);
                    ledgerEntries.Add(current);
                    ledgerCount--;
                    byteIndex += bytesRead;
                }
            }

            return ledgerEntries;
        }

        public string GetRegexMatch(string regex) => String.Concat(".*", regex.TrimStart(), "*");

        // Tried a ton of ways to read and write in the same stream. It was super-fast, but I always
        // had data corruption. Creating a new file is simpler, with no issues.
        public void CategorizeDefaultLedger(IEnumerable<TitleRegex.TitleRegex> titleRegices)
        {
            string ledgerPath = base.GetDefaultPath(_Folder, _schema.Ledger.GetDefaultName());
            string newLedgerPath = base.GetCopyPath(_Folder, _schema.Ledger.GetDefaultName());
            string line;

            using (var reader = new StreamReader(ledgerPath))
            {
                using (var writer = new StreamWriter(newLedgerPath))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        Ledger ledger = _parser.ParseLine(line);

                        foreach (var titleRegex in titleRegices)
                        {
                            string match = GetRegexMatch(titleRegex.Regex);
                            bool isMatch = Regex.IsMatch(ledger.Title, match, RegexOptions.IgnoreCase);

                            if (isMatch)
                            {
                                ledger.Subcategory = titleRegex.Subcategory;
                                continue;
                            }
                        }

                        string newLine = _parser.ParseLedger(ledger);
                        writer.WriteLine(newLine);
                    }
                }
            }

            System.IO.File.Copy(newLedgerPath, ledgerPath, overwrite: true);
            System.IO.File.Delete(newLedgerPath);
        }
    }
}