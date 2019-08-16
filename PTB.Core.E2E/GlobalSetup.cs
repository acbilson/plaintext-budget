using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PTB.Core.Ledger;
using PTB.Core.Statements;
using PTB.Core.TitleRegex;
using System.Collections.Generic;
using System.IO;
using PTB.Core.Categories;
using PTB.Core.Logging;

namespace PTB.Core.E2E
{
    [TestClass]
    public class GlobalSetup
    {
        public PTBSettings CleanSettings;
        public PTBSettings DirtySettings;
        public PTBSchema Schema;
        public PTBClient Client;
        public PNCParser PNCParser;
        public LedgerParser LedgerParser;
        public FileManager FileManager;
        public IPTBLogger Logger;

        public List<Tuple<string, string>> CopiedFiles = new List<Tuple<string, string>>();

        [TestInitialize] 
        public void GlobalInitialize()
        {
            GetDefaultSchema();
            GetDefaultSettings();
            GetDirtySettings();

            // copies a fresh ledger each time since many e2e tests write to it
            CopyLedger();
        }

        #region Initialize

        public void GetDefaultSettings()
        {
            var settings = new PTBSettings
            {
                FileDelimiter = "_",
                FileExtension = ".txt",
                HomeDirectory = @"C:\Users\abilson\SourceCode\PlaintextBudget\TestOutput\netcoreapp2.1\Clean",
                LoggingLevel = LoggingLevel.Info
            };
            CleanSettings = settings;
        }
        public void GetDirtySettings()
        {
            var settings = new PTBSettings
            {
                FileDelimiter = "_",
                FileExtension = ".txt",
                HomeDirectory = @"C:\Users\abilson\SourceCode\PlaintextBudget\TestOutput\netcoreapp2.1\Dirty",
                LoggingLevel = LoggingLevel.Info
            };
            DirtySettings = settings;
        }

        public void GetDefaultSchema()
        {
            var text = File.ReadAllText($@".\schema.json");
            PTBSchema schema = JsonConvert.DeserializeObject<PTBSchema>(text);
            Schema = schema;
        }

        public void CopyLedger()
        {
            string srcPath = $@".\Clean\{Schema.Ledger.Folder}\ledger-base{CleanSettings.FileExtension}";
            string destPath = Path.Combine(CleanSettings.HomeDirectory, Schema.Ledger.Folder, Schema.Ledger.DefaultFileName + CleanSettings.FileExtension);
            File.Copy(srcPath, destPath, overwrite: true);
        }

        #endregion Initialize

        #region Arrange - With

        public void WithAFileManager()
        {
            var fileManager = new FileManager(CleanSettings, Schema);
            FileManager = fileManager;
        }

        public void WithALogger()
        {
            var logger = new PTBFileLogger(CleanSettings.LoggingLevel, CleanSettings.HomeDirectory);
            Logger = logger;
        }

        public void WithAFileClient()
        {
            WithALogger();
            WithAFileManager();
            var client = new PTBClient();
            client.Instantiate(FileManager, Logger);
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
            string path = System.IO.Path.Combine(CleanSettings.HomeDirectory, @"Statements\datafile.csv");
            Client.Ledger.ImportToDefaultLedger(path, PNCParser);
        }

        public void WhenALedgerIsCategorized()
        {
            TitleRegexReadResponse response = Client.Regex.ReadAllTitleRegex();
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
            string path = System.IO.Path.Combine(CleanSettings.HomeDirectory, @"Ledgers\ledger_checking_19-01-01_19-12-31.txt");
            string ledgerEntries = System.IO.File.ReadAllText(path);
            string firstLine = ledgerEntries.Substring(0, Schema.Ledger.LineSize + System.Environment.NewLine.Length);
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
            var response = Client.Ledger.ReadDefaultLedgerEntries(0, 10000);
            return response.Result;
        }

        public List<Categories.Categories> WithAllCategories()
        {
            var categories = Client.Categories.ReadAllDefaultCategories();
            return categories.Categories;
        }

        #endregion Act - With

        #region Assert - Should

        public void ShouldImportAllLedgerEntries()
        {
            string path = System.IO.Path.Combine(CleanSettings.HomeDirectory, @"Ledgers\ledger_checking_19-01-01_19-12-31.txt");
            IEnumerable<string> ledgerEntries = System.IO.File.ReadLines(path);
            // takes all lines minus the header
            Assert.AreEqual(117 - 1, ledgerEntries.Count());
        }

        // first line: 2019/06/18,310.80,"Direct Deposit - Payroll","OPTIMUM JOY CLIN XXXXXXXXXXX39-0","000191699","CREDIT"
        public void ShouldParseFirstEntry(Ledger.Ledger ledger)
        {
            Assert.AreEqual("2019-06-18", ledger.Date);
            Assert.AreEqual("310.80", ledger.Amount.TrimStart());
            Assert.AreEqual("", ledger.Subject.Trim());
            Assert.AreEqual("directdepositpayrolloptimumjoyclinxxxxxxxxxxx390", ledger.Title.TrimStart());
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
            Assert.AreEqual(63, lines.Length);
        }
        public void ShouldGenerateASortedBudget(string[] lines)
        {
            string firstCategory = "Charity";
            string firstSubcategory = "Neighborhood Campus";
            Assert.IsTrue(lines[0].Contains(firstCategory), $"First category should contain the word {firstCategory} if the budget is sorted.");
            Assert.IsTrue(lines[1].Contains(firstSubcategory), $"First subcategory under {firstCategory} should contain the word {firstSubcategory} if the budget is sorted.");
        }

        public void ShouldHaveCategorizedAtLeastOneLedger(List<Ledger.Ledger> ledgerEntries)
        {
            var noSubcategoryLedgers = ledgerEntries.Where(l => l.Subcategory.Trim() == "");
            var noSubjectLedgers = ledgerEntries.Where(l => l.Subject.Trim() == "");
            Assert.AreNotEqual(noSubcategoryLedgers.Count(), ledgerEntries.Count(), "There should not be the same number of ledgers without subcategories as total.");
            Assert.AreNotEqual(noSubjectLedgers.Count(), ledgerEntries.Count(), "There should not be the same number of ledgers without subjects as total.");
        }

        public void ShouldNotCategorizeLockedLedger(List<Ledger.Ledger> ledgerEntries)
        {
            var lockedLedgerEntries = ledgerEntries.Where(l => l.Locked == '1');

            foreach (var ledgerEntry in lockedLedgerEntries)
            {
                Assert.AreEqual("Custom", ledgerEntry.Subcategory.TrimStart());
            }
        }

        public void ShouldNotHaveAnySkippedCategories(CategoriesReadResponse response)
        {
            if (response.SkippedMessages.Count > 0)
            {
                string messages = string.Join(System.Environment.NewLine, response.SkippedMessages);
                Assert.Fail($"There were skipped categories. Messages: {messages}");
            }
            Assert.AreEqual(39, response.Categories.Count());

        }

        #endregion Assert - Should

        #region Assert - With

        public string[] WithBudgetLines()
        {
            string fileName = Client.Budget.GetBudgetName();
            string path = System.IO.Path.Combine(CleanSettings.HomeDirectory, $@"Budget\{fileName}{CleanSettings.FileExtension}");
            string[] budgetLines = System.IO.File.ReadAllLines(path);
            return budgetLines;
        }

        #endregion Assert - With

        // should be the full length of the line plus ending (117) multiplied by the line number minus 1 b/c it starts a 1
        private int CalculateLedgerIndex(int lineNumber) => (Schema.Ledger.LineSize + System.Environment.NewLine.Length) * (lineNumber - 1);

        private Ledger.Ledger GetLedgerOnLine(int lineNumber)
        {
            string path = System.IO.Path.Combine(CleanSettings.HomeDirectory, @"Ledgers\ledger_checking_19-01-01_19-12-31.txt");
            string ledgerEntries = System.IO.File.ReadAllText(path);
            int ledgerIndex = CalculateLedgerIndex(lineNumber);
            string line = ledgerEntries.Substring(ledgerIndex, Schema.Ledger.LineSize + System.Environment.NewLine.Length);
            StringToLedgerResponse response = LedgerParser.ParseLine(line, ledgerIndex);

            Assert.IsTrue(response.Success, $"Failed to parse ledger with message {response.Message}");
            return response.Result;
        }
    }
}