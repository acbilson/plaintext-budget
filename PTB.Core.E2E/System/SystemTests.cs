using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PTB.Core.Ledger;
using PTB.Core.Statements;
using PTB.Core.TitleRegex;
using System.Collections.Generic;
using System.IO;
using PTB.Core.E2E;

namespace PTB.Core.PTBSystem
{
    [TestClass]
    public class SystemTests : GlobalSetup
    {
        [TestMethod]
        public void FullSystemTest()
        {
            // Arrange
            WithAFileClient();
            WithAPNCParser();

            // Act - Import
            WhenACleanStatementIsImported();

            // Assert - Import
            ShouldImportAllLedgerEntries();

            // Act - Categorize
            WhenALedgerIsCategorized();
            List<Ledger.Ledger> ledgerEntries = WithAllLedgerEntries();

            // Assert - Categorize
            ShouldNotCategorizeLockedLedger(ledgerEntries);
            ShouldHaveCategorizedAtLeastOneLedger(ledgerEntries);

            // Act - Read
            var categoriesResponse = Client.Categories.ReadAllDefaultCategories();
            var ledgerResponse = Client.Ledger.ReadDefaultLedgerEntries(0, 10000);

            // Assert - Read
            ShouldNotHaveAnySkippedCategories(categoriesResponse);

            // Act - Budget
            Client.Budget.CreateBudget(categoriesResponse.Categories);

            // Assert - Budget
            string[] lines = WithBudgetLines();
            ShouldGenerateABudgetOfTheRightSize(lines);
            ShouldGenerateASortedBudget(lines);
        }
    }
}
