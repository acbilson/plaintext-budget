using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using System.Collections.Generic;
using System.Linq;

namespace PTB.Core.E2E
{
    [TestClass]
    public class CategorizeTests : GlobalSetup
    {
        [TestMethod]
        public void CategorizesCorrectLedgerEntries()
        {
            // Arrange
            WithALedgerService();
            WithACategoriesService();
            WithATitleRegexService();

            // Act
            WhenALedgerIsCategorized();
            List<PTBRow> ledgerEntries = WithAllLedgerEntries();

            // Assert
            ShouldHaveCategorizedAtLeastOneLedger(ledgerEntries);
            ShouldNotCategorizeLockedLedger(ledgerEntries);
        }
    }
}