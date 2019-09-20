using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;

namespace PTB.E2E.Import
{
    [TestClass]
    [TestCategory("integration")]
    public class ImportStatementTests : GlobalSetup
    {
        [TestMethod]
        public void ImportsEntireStatement()
        {
            // Arrange

            // Act
            WhenACleanStatementIsImported();

            // Assert
            ShouldImportAllLedgerEntries();
        }

        [TestMethod]
        public void ImportsParsableStatement()
        {
            // Arrange

            // Act
            WhenACleanStatementIsImported();
            PTBRow ledger = WithTheFirstParsedLedger();

            // Assert
            ShouldParseFirstEntry(ledger);
        }
    }
}