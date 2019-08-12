using PTB.File.Statements;
using System.Collections.Generic;
using System.IO;

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

        public void CategorizeDefaultLedger(IEnumerable<TitleRegex.TitleRegex> titleRegices)
        {
            string ledgerPath = base.GetDefaultPath(_Folder, _schema.Ledger.GetDefaultName());

            using (LedgerFile defaultLedgerFile = new LedgerFile(ledgerPath))
            {
                defaultLedgerFile.Categorize(_parser, _schema.Ledger.Size, titleRegices);
            }
        }
    }
}