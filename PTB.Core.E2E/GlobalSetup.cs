using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PTB.Core.Ledger;
using PTB.Core.Statements;
using PTB.Core.TitleRegex;
using System.Collections.Generic;

namespace PTB.Core.E2E
{
    [TestClass]
    public class GlobalSetup
    {
        public PTBSettings Settings;
        public PTBSchema Schema;
        public PTBClient Client;
        public PNCParser PNCParser;
        public LedgerParser LedgerParser;
        public FileManager FileManager;

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
            var client = new PTBClient();
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

        public void WithAFileManager()
        {
            var fileManager = new FileManager(Settings, Schema);
            FileManager = fileManager;
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
            CategoriesReadResponse response = Client.Regex.ReadAllTitleRegex();
            Client.Ledger.CategorizeDefaultLedger(response.TitleRegices);
        }

        public LedgerUpdateResponse WhenALedgerIsUpdated(Ledger.Ledger ledgerToUpdate)
        {
            var response = Client.Ledger.UpdateDefaultLedgerEntry(ledgerToUpdate);
            return response;
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

        public Ledger.Ledger WithTheFourthLedger()
        {
            var ledger = GetLedgerOnLine(4);
            return ledger;
        }

        public List<Ledger.Ledger> WithAllLedgerEntries()
        {
            var entries = Client.Ledger.ReadDefaultLedgerEntries(0, 10000);
            return entries;
        }

        public List<Categories.Categories> WithAllCategories()
        {
            var categories = Client.Categories.ReadAllCategories();
            return categories.Categories;
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

        public void ShouldUpdateFourthEntryWithNewSubcategory(string subcategory)
        {
            var ledger = GetLedgerOnLine(4);
            Assert.AreEqual(subcategory, ledger.Subcategory);
        }

        public void ShouldGenerateABudgetOfTheRightSize(string[] lines)
        {
            Assert.AreEqual(64, lines.Length);
        }
        public void ShouldGenerateASortedBudget(string[] lines)
        {
            string firstCategory = "Charity";
            string firstSubcategory = "Neighborhood Campus";
            Assert.IsTrue(lines[0].Contains(firstCategory), $"First category should contain the word {firstCategory} if the budget is sorted.");
            Assert.IsTrue(lines[1].Contains(firstSubcategory), $"First subcategory under {firstCategory} should contain the word {firstSubcategory} if the budget is sorted.");
        }

        #endregion Assert - Should

        #region Assert - With

        public string[] WithBudgetLines()
        {
            string fileName = Client.Budget.GetBudgetName();
            string path = System.IO.Path.Combine(Settings.HomeDirectory, $@"Budget\{fileName}{Settings.FileExtension}");
            string[] budgetLines = System.IO.File.ReadAllLines(path);
            return budgetLines;
        }

        #endregion Assert - With

        // should be the full length of the line plus ending (117) multiplied by the line number minus 1 b/c it starts a 1
        private int CalculateLedgerIndex(int lineNumber) => (Schema.Ledger.Size + System.Environment.NewLine.Length) * (lineNumber - 1);

        private Ledger.Ledger GetLedgerOnLine(int lineNumber)
        {
            string path = System.IO.Path.Combine(Settings.HomeDirectory, @"Ledgers\ledger_checking_19-01-01_19-12-31.txt");
            string ledgerEntries = System.IO.File.ReadAllText(path);
            int ledgerIndex = CalculateLedgerIndex(lineNumber);
            string line = ledgerEntries.Substring(ledgerIndex, Schema.Ledger.Size + System.Environment.NewLine.Length);
            StringToLedgerResponse response = LedgerParser.ParseLine(line, ledgerIndex);

            Assert.IsTrue(response.Success, $"Failed to parse ledger with message {response.Message}");
            return response.Result;
        }
    }
}