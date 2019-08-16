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
            Assert.IsTrue(ledgerEntries.Any((l) => l.Subcategory.TrimStart() == "Coffee"), $"Failed to categorize any ledger entries as Coffee.");
            Assert.IsTrue(ledgerEntries.Any((l) => l.Subcategory.TrimStart() == "Groceries"), "Failed to categorize any ledger entries as Groceries");
        }
    }
}