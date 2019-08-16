using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTB.Core.E2E
{
    [TestClass]
    public class ImportStatementTests : GlobalSetup
    {
        [TestMethod]
        public void ImportsEntireStatement()
        {
            // Arrange
            WithAFileClient();
            WithAPNCParser();

            // Act
            WhenACleanStatementIsImported();

            // Assert
            ShouldImportAllLedgerEntries();
        }

        [TestMethod]
        public void ImportsParsableStatement()
        {
            // Arrange
            WithAFileClient();
            WithAPNCParser();
            WithALedgerParser();

            // Act
            WhenACleanStatementIsImported();
            Ledger.Ledger ledger = WithTheFirstParsedLedger();

            // Assert
            ShouldParseFirstEntry(ledger);
        }
    }
}