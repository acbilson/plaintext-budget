using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using PTB.Core.E2E;
using PTB.Files.Ledger;
using PTB.Files.TitleRegex;
using System.Collections.Generic;

namespace PTB.Core.PTBSystem
{
    [TestClass]
    [TestCategory("integration")]
    public class SystemTests : GlobalSetup
    {
        [TestMethod]
        public void FullSystemTest()
        {
            // Arrange
            var ledgerService = Provider.GetService<LedgerService>();
            var titleRegexService = Provider.GetService<TitleRegexService>();

            // Act - Import
            WhenACleanStatementIsImported();

            // Assert - Import
            ShouldImportAllLedgerEntries();

            // Act - Categorize
            WhenALedgerIsCategorized();
            List<PTBRow> ledgerEntries = WithAllLedgerEntries();

            // Assert - Categorize
            ShouldNotCategorizeLockedLedger(ledgerEntries);
            ShouldHaveCategorizedAtLeastOneLedger(ledgerEntries);

            // Act - Read
            //var defaultCategoriesFile = FileFolders.CategoriesFolder.GetDefaultFile();
            //var defaultLedgerFile = FileFolders.LedgerFolder.GetDefaultFile();
            //var categoriesResponse = categoriesService.Read(defaultCategoriesFile, 0, defaultCategoriesFile.LineCount);
            //var ledgerResponse = ledgerService.Read(defaultLedgerFile, 0, defaultLedgerFile.LineCount);

            // Assert - Read
            //ShouldNotHaveAnySkippedCategories(categoriesResponse);

            // Act - Budget
            //Client.Budget.CreateBudget(categoriesResponse.Categories);

            // Assert - Budget
            //string[] lines = WithBudgetLines();
            //ShouldGenerateABudgetOfTheRightSize(lines);
            //ShouldGenerateASortedBudget(lines);
        }
    }
}