using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PTB.File.Base;
using PTB.File.Ledger;
using PTB.File.Statements;
using PTB.File.TitleRegex;
using System.Collections.Generic;

namespace PTB.File.E2E
{
    [TestClass]
    public class GlobalSetup
    {
        public PTBSettings Settings;
        public PTBSchema Schema;
        public FileClient Client;
        public PNCParser PNCParser;
        public LedgerParser LedgerParser;

        #region Initialize
        public void GetDefaultSettings(string folder)
        {
            var text = System.IO.File.ReadAllText($@".\{folder}\settings.json");
            PTBSettings settings = JsonConvert.DeserializeObject<PTBSettings>(text);
            Settings = settings;
        }

        public void GetDefaultSchema(string folder)
        {
            var text = System.IO.File.ReadAllText($@".\{folder}\schema.json");
            PTBSchema schema = JsonConvert.DeserializeObject<PTBSchema>(text);
            Schema = schema;
            /*
            var schema = new PTBSchema
            {
                Ledger = new LedgerSchema
                {
                    Columns = new LedgerColumns
                    {
                        Date =        new ColumnDate {               Index = 1, Size = 10, Offset =   0  },
                        Type =        new ColumnType {               Index = 2, Size =  1, Offset =  10 },
                        Amount =      new ColumnAmount {             Index = 3, Size = 12, Offset =  11 },
                        Subcategory = new Ledger.ColumnSubcategory { Index = 4, Size = 20, Offset =  23 },
                        Title =       new ColumnTitle {              Index = 5, Size = 50, Offset =  43 },
                        Location =    new ColumnLocation {           Index = 6, Size = 15, Offset =  93 },
                        Locked =      new ColumnLocked {             Index = 7, Size =  1, Offset = 108 }
                    },
                    Delimiter = " ",
                    Files = new PTBFiles[] {
                        new PTBFiles { IsDefault = true, Name = "ledger_checking_19-01-01_19-12-31" }
                    },
                    Size = 115
                },
                TitleRegex = new TitleRegexSchema
                {
                    Columns = new TitleRegexColumns
                    {
                        Priority =    new ColumnPriority {               Index = 1, Size =  1, Offset =  0 },
                        Subcategory = new TitleRegex.ColumnSubcategory { Index = 2, Size = 30, Offset =  1 },
                        Regex =       new ColumnRegex {                  Index = 3, Size = 30, Offset = 31 }
                    },
                    Delimiter = " ",
                    Files = new PTBFiles[] {
                        new PTBFiles { IsDefault = true, Name = "regex-title" }
                    },
                    Size = 63
                }
            };
            */
        }
        #endregion Initialize

        #region Arrange - With
        public void WithAFileClient()
        {
            var client = new FileClient();
            client.Instantiate(Settings.HomeDirectory);
            Client = client;
        }

        public void WithAPNCParser()
        {
            var parser = new PNCParser();
            PNCParser = parser;
        }

        public void WithALedgerParser()
        {
            var parser = new LedgerParser(Schema.Ledger);
            LedgerParser = parser;
        }
        #endregion Arrange - With

        
        #region Act - When
        public void WhenACleanStatementIsImported()
        {
            string path = System.IO.Path.Combine(Settings.HomeDirectory, @"Clean\datafile.csv");
            Client.Ledger.ImportToDefaultLedger(path, PNCParser);
        }

        public void WhenALedgerIsCategorized()
        {
            IEnumerable<TitleRegex.TitleRegex> titleRegices = Client.Regex.ReadAllTitleRegex();
            Client.Ledger.CategorizeDefaultLedger(titleRegices);
        }
        #endregion Act - When

        #region Act - With

        public Ledger.Ledger WithTheFirstParsedLedger()
        {
            string path = System.IO.Path.Combine(Settings.HomeDirectory, @"Ledgers\ledger_checking_19-01-01_19-12-31.txt");
            string ledgerEntries = System.IO.File.ReadAllText(path);
            string firstLine = ledgerEntries.Substring(0, Schema.Ledger.Size + System.Environment.NewLine.Length);
            StringToLedgerResponse response = LedgerParser.ParseLine(firstLine, 0);
            return response.Result; 
        }

        public List<Ledger.Ledger> WithAllLedgerEntries()
        {
            var entries = Client.Ledger.ReadDefaultLedgerEntries(0, 10000);
            return entries;
        }

        #endregion Act - With

        #region Assert - Should
        public void ShouldImportAllLedgerEntries()
        {
            string path = System.IO.Path.Combine(Settings.HomeDirectory, @"Ledgers\ledger_checking_19-01-01_19-12-31.txt");
            string ledgerEntries = System.IO.File.ReadAllText(path);
            Assert.AreEqual(13572, ledgerEntries.Length);
        }

        // first line: 2019/06/18,310.80,"Direct Deposit - Payroll","OPTIMUM JOY CLIN XXXXXXXXXXX39-0","000191699","CREDIT"
        public void ShouldParseFirstEntry(Ledger.Ledger ledger)
        {
            Assert.AreEqual("2019-06-18", ledger.Date);
            Assert.AreEqual("310.80", ledger.Amount.TrimStart());
            Assert.AreEqual("directdepositpayrolloptimumjoyclinxxxxxxxxxxx390", ledger.Title.TrimStart());
            Assert.AreEqual("000191699", ledger.Location.TrimStart());
            Assert.AreEqual('C', ledger.Type);
            Assert.AreEqual('0', ledger.Locked);
            Assert.AreEqual("", ledger.Subcategory.Trim());
        }

        #endregion Assert - Should

    }
}
