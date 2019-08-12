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