using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PTB.Core.Statements;
using System.Collections.Generic;
using System.IO;
using PTB.Core.E2E;
using PTB.Core.Base;

namespace PTB.Core.PTBSystem
{
    [TestClass]
    public class SystemTests : GlobalSetup
    {
        [TestMethod]
        public void FullSystemTest()
        {
            // Arrange
            WithAPNCParser();
            WithALedgerService();
            WithACategoriesService();
            WithATitleRegexService();

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
            var categoriesResponse = CategoriesService.Read(defaultCategoriesFile, 0, defaultCategoriesFile.LineCount);
            var ledgerResponse = LedgerService.Read(defaultLedgerFile, 0, defaultLedgerFile.LineCount);

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
