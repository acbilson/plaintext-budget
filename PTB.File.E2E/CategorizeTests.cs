using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.File.E2E
{
    [TestClass]
    public class CategorizeTests : GlobalSetup
    {
        [TestInitialize]
        public void Initialize()
        {
            string folder = "Categorize";
            GetDefaultSchema(folder);
            GetDefaultSettings(folder);
        }

        [TestMethod]
        public void CategorizesCorrectLedgerEntries()
        {
            // Arrange
            WithAFileClient();

            // Act
            WhenALedgerIsCategorized();
            List<Ledger.Ledger> ledgerEntries = WithAllLedgerEntries();

            // Assert
            Assert.IsTrue(ledgerEntries.Any((l) => l.Subcategory.TrimStart() == "Restaurant"), $"Failed to categorize any ledger entries as Restaurant.");
            Assert.IsTrue(ledgerEntries.Any((l) => l.Subcategory.TrimStart() == "Groceries"), "Failed to categorize any ledger entries as Groceries");
        }
    }
}
