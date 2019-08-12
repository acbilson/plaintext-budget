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

        public void ImportToDefaultLedger(string path, IStatementParser parser)
        {
            string ledgerPath = base.GetDefaultPath(_Folder, _schema.Ledger.GetDefaultName());

            using (LedgerFile defaultLedgerFile = new LedgerFile(ledgerPath))
            {
                string line;
                using (StreamReader reader = new StreamReader(path))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        ParseResponse response = parser.ParseLine(line, _schema.Ledger);

                        if (response.Success)
                        {
                            defaultLedgerFile.WriteLine(response.Result);
                        }
                    }
                }
            }
        }

        public List<Ledger> ReadDefaultLedgerEntries()
        {
            var ledgerEntries = new List<Ledger>();
            string ledgerPath = base.GetDefaultPath(_Folder, _schema.Ledger.GetDefaultName());

            using (var reader = new StreamReader(ledgerPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Ledger current = _parser.ParseLine(line);
                    ledgerEntries.Add(current);
                }
            }

            return ledgerEntries;
        }

        public string GetRegexMatch(string regex) => String.Concat(".*", regex.TrimStart(), "*");

        public void CategorizeDefaultLedger(IEnumerable<TitleRegex.TitleRegex> titleRegices)
        {
            string ledgerPath = base.GetDefaultPath(_Folder, _schema.Ledger.GetDefaultName());

            using (var stream = System.IO.File.Open(ledgerPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                var reader = new StreamReader(stream, Encoding.UTF8);
                var writer = new StreamWriter(stream, Encoding.UTF8);
                writer.AutoFlush = true;

                int bufferSize = _schema.Ledger.Size + Constant.WIN_LINE_ENDING_SIZE;
                char[] buffer = new char[bufferSize];

                int lineCount = 1;
                while (reader.Read(buffer, 0, buffer.Length) != 0)
                {
                    stream.Position = buffer.Length * lineCount;

                    Ledger current = _parser.ParseLine(new String(buffer));

                    foreach (var titleRegex in titleRegices)
                    {
                        string match = GetRegexMatch(titleRegex.Regex);
                        if (Regex.IsMatch(current.Title, match, RegexOptions.IgnoreCase))
                        {
                            current.Subcategory = titleRegex.Subcategory;

                            // returns stream to overwrite
                            stream.Position -= buffer.Length - (Constant.WIN_LINE_ENDING_SIZE + 1); // not sure why the extra byte
                            string newLine = _parser.ParseLedger(current);
                            writer.Write(newLine);

                            // moves to next line
                            continue;
                        }
                    }

                    lineCount++;
                }
            }
        }
    }
}