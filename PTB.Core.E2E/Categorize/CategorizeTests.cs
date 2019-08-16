using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            WithAFileClient();

            // Act
            WhenALedgerIsCategorized();
            List<Ledger.Ledger> ledgerEntries = WithAllLedgerEntries();

            // Assert
            ShouldHaveCategorizedAtLeastOneLedger(ledgerEntries);
            ShouldNotCategorizeLockedLedger(ledgerEntries);
        }
    }
}