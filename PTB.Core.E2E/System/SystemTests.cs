using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PTB.Core.Statements;
using System.Collections.Generic;
using System.IO;
using PTB.Core.E2E;
using PTB.Core.Base;
using PTB.Files.Ledger;
using PTB.Files.Categories;
using PTB.Files.TitleRegex;

namespace PTB.Core.PTBSystem
{
    [TestClass]
    public class SystemTests : GlobalSetup
    {
        [TestMethod]
        public void FullSystemTest()
        {
            // Arrange
            var ledgerService = Provider.GetService<LedgerService>();
            var categoriesService = Provider.GetService<CategoriesService>();
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
            var defaultCategoriesFile = FileFolders.CategoriesFolder.GetDefaultFile();
            var defaultLedgerFile = FileFolders.LedgerFolder.GetDefaultFile();
            var categoriesResponse = categoriesService.Read(defaultCategoriesFile, 0, defaultCategoriesFile.LineCount);
            var ledgerResponse = ledgerService.Read(defaultLedgerFile, 0, defaultLedgerFile.LineCount);

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
