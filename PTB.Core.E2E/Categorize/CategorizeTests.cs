using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace PTB.Core.E2E
{
    [TestClass]
    public class CategorizeTests : GlobalSetup
    {
        [TestInitialize]
        public void Initialize()
        {
            List<string> filesToCopy = new List<string> { @"./Clean/Ledgers/ledger_checking_19-01-01_19-12-31.txt" };
            CopyFiles(filesToCopy);
        }

        [TestCleanup]
        public void Cleanup()
        {
            RestoreFiles();
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
            Assert.IsTrue(ledgerEntries.Any((l) => l.Subcategory.TrimStart() == "Coffee"), $"Failed to categorize any ledger entries as Coffee.");
            Assert.IsTrue(ledgerEntries.Any((l) => l.Subcategory.TrimStart() == "Groceries"), "Failed to categorize any ledger entries as Groceries");
        }
    }
}