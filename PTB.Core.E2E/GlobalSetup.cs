using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PTB.Categories.Categories;
using PTB.Core;
using PTB.Core.Base;
using PTB.Core.Logging;
using PTB.Core.Statements;
using PTB.Files;
using PTB.Files.FolderAccess;
using PTB.Files.Ledger;
using PTB.Files.Statements;
using PTB.Files.TitleRegex;
using PTB.Report;
using PTB.Reports.Budget;
using PTB.Reports.Categories;
using PTB.Reports.FolderAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PTB.E2E
{
    [TestClass]
    public class GlobalSetup
    {
        public PTBSettings CleanSettings;
        public PTBSettings DirtySettings;
        public FileSchema FileSchema;
        public ReportSchema ReportSchema;
        public PTBFileLogger Logger;

        public List<Tuple<string, string>> CopiedFiles = new List<Tuple<string, string>>();
        public FileFolders FileFolders;
        public ReportFolders ReportFolders;
        public ServiceProvider Provider;

        [TestInitialize]
        public void GlobalInitialize()
        {
            GetDefaultSettings();
            GetDirtySettings();
            GetDefaultSchema();
            WithALogger();
            WithAServiceCollection();

            // copies a fresh ledger and budget each time since many e2e tests write to it
            CopyLedger();
            CopyBudget();

            // reads all files
            WithFileFolders();
            WithReportFolders();
        }

        #region Initialize

        public void GetDefaultSettings()
        {
            var settings = new PTBSettings
            {
                FileDelimiter = '_',
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
                FileDelimiter = '_',
                FileExtension = ".txt",
                HomeDirectory = @"C:\Users\abilson\SourceCode\PlaintextBudget\TestOutput\netcoreapp2.1\Dirty",
                LoggingLevel = LoggingLevel.Info
            };
            DirtySettings = settings;
        }

        public void GetDefaultSchema()
        {
            var text = File.ReadAllText(Path.Combine(CleanSettings.HomeDirectory, "schema.json"));
            FileSchema = JsonConvert.DeserializeObject<FileSchema>(text);
            ReportSchema = JsonConvert.DeserializeObject<ReportSchema>(text);
        }

        public void CopyLedger()
        {
            string srcPath = Path.Combine(CleanSettings.HomeDirectory, $@"{FileSchema.Ledger.Folder}\ledger-base{CleanSettings.FileExtension}");
            string destPath = Path.Combine(CleanSettings.HomeDirectory, FileSchema.Ledger.Folder, FileSchema.Ledger.DefaultFileName + CleanSettings.FileExtension);
            File.Copy(srcPath, destPath, overwrite: true);
        }

        public void CopyBudget()
        {
            string srcPath = Path.Combine(CleanSettings.HomeDirectory, $@"{ReportSchema.Budget.Folder}\budget-base{CleanSettings.FileExtension}");
            string destPath = Path.Combine(CleanSettings.HomeDirectory, ReportSchema.Budget.Folder, ReportSchema.Budget.DefaultFileName + CleanSettings.FileExtension);
            File.Copy(srcPath, destPath, overwrite: true);
        }

        public void WithFileFolders()
        {
            var fileFolderService = Provider.GetService<FileFolderService>();
            FileFolders = fileFolderService.GetFolders();
        }

        public void WithReportFolders()
        {
            var reportFolderService = Provider.GetService<ReportFolderService>();
            ReportFolders = reportFolderService.GetFolders();
        }

        #endregion Initialize

        #region Arrange - With

        public void WithAServiceCollection()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IPTBLogger>(Logger)

                .AddSingleton<PTBSettings>(CleanSettings)

                .AddSingleton<FileSchema>(FileSchema)
                .AddSingleton<LedgerSchema>(FileSchema.Ledger)
                .AddSingleton<TitleRegexSchema>(FileSchema.TitleRegex)

                .AddSingleton<ReportSchema>(ReportSchema)
                .AddSingleton<BudgetSchema>(ReportSchema.Budget)
                .AddSingleton<CategoriesSchema>(ReportSchema.Categories)

                .AddSingleton<IStatementParser, PNCParser>()
                .AddSingleton<LedgerFileParser>()
                .AddSingleton<CategoriesFileParser>()
                .AddSingleton<TitleRegexFileParser>()
                .AddSingleton<BudgetReportParser>()

                .AddScoped<FileFolderService>()
                .AddScoped<ReportFolderService>()
                .AddScoped<BudgetFolderService>()

                .AddScoped<LedgerService>()
                .AddScoped<CategoriesService>()
                .AddScoped<TitleRegexService>()
                .AddScoped<BudgetService>()
            .BuildServiceProvider();

            Provider = serviceProvider;
        }

        public void WithALogger()
        {
            var logger = new PTBFileLogger(CleanSettings.LoggingLevel, CleanSettings.HomeDirectory);
            Logger = logger;
        }

        #endregion Arrange - With

        #region Act - When

        public void WhenAllFileFoldersHaveBeenRetrieved()
        {
            var fileFolderService = Provider.GetService<FileFolderService>();
            var fileFolders = fileFolderService.GetFolders();
            FileFolders = fileFolders;
        }

        public void WhenACleanStatementIsImported()
        {
            string path = System.IO.Path.Combine(CleanSettings.HomeDirectory, @"Statements\datafile.csv");
            var defaultLedgerFile = FileFolders.LedgerFolder.GetDefaultFile();

            var statementParser = Provider.GetService<IStatementParser>();
            var ledgerService = Provider.GetService<LedgerService>();
            ledgerService.Import(defaultLedgerFile, path, statementParser);
        }

        public void WhenALedgerIsCategorized()
        {
            var defaultTitleRegexFile = FileFolders.TitleRegexFolder.GetDefaultFile();
            var titleRegexService = Provider.GetService<TitleRegexService>();
            BaseReadResponse response = titleRegexService.Read(defaultTitleRegexFile);

            var defaultLedgerFile = FileFolders.LedgerFolder.GetDefaultFile();
            var ledgerService = Provider.GetService<LedgerService>();
            ledgerService.Categorize(defaultLedgerFile, response.ReadResult);
        }

        public BaseUpdateResponse WhenALedgerIsUpdated(int index, PTBRow ledgerToUpdate)
        {
            var defaultLedgerFile = FileFolders.LedgerFolder.GetDefaultFile();
            var ledgerService = Provider.GetService<LedgerService>();
            var response = ledgerService.Update(defaultLedgerFile, index, ledgerToUpdate);
            return response;
        }

        public BaseUpdateResponse WhenABudgetIsUpdated(int index, PTBRow recordToUpdate)
        {
            var defaultBudgetFile = ReportFolders.BudgetFolder.GetDefaultFile();
            var budgetService = Provider.GetService<BudgetService>();
            var response = budgetService.Update(defaultBudgetFile, index, recordToUpdate);
            return response;
        }

        public BaseUpdateResponse WhenABudgetRecordIsUpdated(int index, PTBRow recordToUpdate)
        {
            var defaultBudgetFile = ReportFolders.BudgetFolder.GetDefaultFile();
            var budgetService = Provider.GetService<BudgetService>();
            var response = budgetService.Update(defaultBudgetFile, index, recordToUpdate);
            return response;
        }

        #endregion Act - When

        #region Act - With

        public PTBRow WithTheFirstParsedLedger()
        {
            var ledgerFileParser = Provider.GetService<LedgerFileParser>();
            string path = System.IO.Path.Combine(CleanSettings.HomeDirectory, @"Ledgers\ledger_checking_19-01-01_19-12-31.txt");
            string ledgerEntries = System.IO.File.ReadAllText(path);
            string firstLine = ledgerEntries.Substring(0, FileSchema.Ledger.LineSize + System.Environment.NewLine.Length);
            var response = ledgerFileParser.ParseLine(firstLine, 0);
            return response.Row;
        }

        public PTBRow WithTheFourthLedger()
        {
            var ledger = GetLedgerOnLine(4);
            return ledger;
        }

        public List<PTBRow> WithAllBudgetRecords()
        {
            var defaultBudgetFile = ReportFolders.BudgetFolder.GetDefaultFile();
            var budgetService = Provider.GetService<BudgetService>();
            var response = budgetService.Read(defaultBudgetFile, 0, defaultBudgetFile.LineCount);
            return response.ReadResult;
        }

        public List<PTBRow> WithAllLedgerEntries()
        {
            var defaultLedgerFile = FileFolders.LedgerFolder.GetDefaultFile();
            var ledgerService = Provider.GetService<LedgerService>();
            var response = ledgerService.Read(defaultLedgerFile, 0, defaultLedgerFile.LineCount);
            return response.ReadResult;
        }

        public List<PTBRow> WithAllCategories()
        {
            var defaultCategoriesFile = ReportFolders.CategoriesFolder.GetDefaultFile();
            var categoriesService = Provider.GetService<CategoriesService>();
            var categories = categoriesService.Read(defaultCategoriesFile);
            return categories.ReadResult;
        }

        public string[] WithAllBudgetLines(string path)
        {
            return File.ReadAllLines(path);
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
        public void ShouldParseFirstEntry(PTBRow ledger)
        {
            Assert.AreEqual("2019-06-18", ledger["date"]);
            Assert.AreEqual("310.80", ledger["amount"].TrimStart());
            Assert.AreEqual("", ledger["subcategory"].Trim());
            Assert.AreEqual("directdepositpayrolloptimumjoyclinxxxxxxxxxxx390", ledger["title"].TrimStart());
            Assert.AreEqual("C", ledger["type"]);
            Assert.AreEqual("0", ledger["locked"]);
            Assert.AreEqual("", ledger["subject"].Trim());
        }

        public void ShouldUpdateFourthEntryWithNewSubcategory(string subcategory)
        {
            var ledger = GetLedgerOnLine(4);
            Assert.AreEqual(subcategory, ledger["subcategory"]);
        }

        public void ShouldNotUpdateFourthEntryWithNewAmount(string amount)
        {
            var ledger = GetLedgerOnLine(4);
            Assert.AreNotEqual(amount, ledger["amount"]);
        }

        public void ShouldGenerateABudgetOfTheRightSize(string[] lines)
        {
            Assert.AreEqual(62, lines.Length);
        }

        public void ShouldGenerateASortedBudget(string[] lines)
        {
            string firstCategory = "Charity";
            string firstSubcategory = "Neighborhood Campus";
            Assert.IsTrue(lines[0].Contains(firstCategory), $"First category should contain the word {firstCategory} if the budget is sorted.");
            Assert.IsTrue(lines[1].Contains(firstSubcategory), $"First subcategory under {firstCategory} should contain the word {firstSubcategory} if the budget is sorted.");
        }

        public void ShouldHaveCategorizedAtLeastOneLedger(List<PTBRow> ledgerEntries)
        {
            var noSubcategoryLedgers = ledgerEntries.Where(l => l["subcategory"].Trim() == "");
            var noSubjectLedgers = ledgerEntries.Where(l => l["subject"].Trim() == "");
            Assert.AreNotEqual(noSubcategoryLedgers.Count(), ledgerEntries.Count(), "There should not be the same number of ledgers without subcategories as total.");
            Assert.AreNotEqual(noSubjectLedgers.Count(), ledgerEntries.Count(), "There should not be the same number of ledgers without subjects as total.");
        }

        public void ShouldNotCategorizeLockedLedger(List<PTBRow> ledgerEntries)
        {
            var lockedLedgerEntries = ledgerEntries.Where(l => l["locked"] == "1");

            foreach (var ledgerEntry in lockedLedgerEntries)
            {
                Assert.AreEqual("Custom", ledgerEntry["subcategory"].TrimStart());
            }
        }

        public void ShouldNotHaveAnySkippedCategories(CategoriesReadResponse response)
        {
            if (response.SkippedMessages.Count > 0)
            {
                string messages = string.Join(System.Environment.NewLine, response.SkippedMessages);
                Assert.Fail($"There were skipped categories. Messages: {messages}");
            }
            Assert.AreEqual(39, response.ReadResult.Count());
        }

        #endregion Assert - Should

        // should be the full length of the line plus ending (117) multiplied by the line number minus 1 b/c it starts a 1
        private int CalculateLedgerIndex(int lineNumber) => (FileSchema.Ledger.LineSize + System.Environment.NewLine.Length) * (lineNumber - 1);

        private PTBRow GetLedgerOnLine(int lineNumber)
        {
            var ledgerFileParser = Provider.GetService<LedgerFileParser>();
            string path = System.IO.Path.Combine(CleanSettings.HomeDirectory, @"Ledgers\ledger_checking_19-01-01_19-12-31.txt");
            string ledgerEntries = System.IO.File.ReadAllText(path);
            int ledgerIndex = CalculateLedgerIndex(lineNumber);
            string line = ledgerEntries.Substring(ledgerIndex, FileSchema.Ledger.LineSize + System.Environment.NewLine.Length);
            var response = ledgerFileParser.ParseLine(line, ledgerIndex);

            Assert.IsTrue(response.Success, $"Failed to parse ledger with message {response.Message}");
            return response.Row;
        }
    }
}