using System;
using System.Collections.Generic;
using System.Text;
using PTB.File;
using PTB.Core;
using Newtonsoft.Json;
using System.IO;
using PTB.Core.Parsers;

namespace PTB.File.Ledger
{
    public class LedgerRepository : BaseFileRepository
    {
        private string _Folder = "Ledgers";
        private PTBSchema _schema;
        private PTBSettings _Settings;
        private LedgerParser _parser;

        public LedgerRepository(PTBSettings settings)
        {
            _Settings = settings;
            _schema = base.ReadFileSchema(_Settings.HomeDirectory);
            _parser = new LedgerParser(_schema.Ledger);
        }

        public void ImportToDefaultLedger(string path, IStatementParser parser)
        {
            string ledgerPath = base.GetDefaultPath(_Settings.HomeDirectory, _Folder, _schema.Ledger.GetDefaultName());

            using (LedgerFile defaultLedgerFile = new LedgerFile(ledgerPath))
            {
                string line;
                using (StreamReader reader = new StreamReader(path))
                {
                    while ((line = reader.ReadLine()) != null) {
                        string transaction = parser.ParseLine(line);
                        defaultLedgerFile.WriteLine(transaction);
                    }
                }
            }
        }

        public void CategorizeDefaultLedger(IEnumerable<TitleRegex.TitleRegex> titleRegices)
        {
            string ledgerPath = base.GetDefaultPath(_Settings.HomeDirectory, _Folder, _schema.Ledger.GetDefaultName());

            using (LedgerFile defaultLedgerFile = new LedgerFile(ledgerPath))
            {
                defaultLedgerFile.Categorize(_parser, _schema.Ledger.Size, titleRegices);
            }
        }
    }
}
