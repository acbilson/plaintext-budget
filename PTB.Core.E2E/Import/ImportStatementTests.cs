using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;

namespace PTB.Core.E2E
{
    [TestClass]
    public class ImportStatementTests : GlobalSetup
    {
        [TestMethod]
        public void ImportsEntireStatement()
        {
            // Arrange
            WithAPNCParser();
            WithALedgerService();

            // Act
            WhenACleanStatementIsImported();

            // Assert
            ShouldImportAllLedgerEntries();
        }

        [TestMethod]
        public void ImportsParsableStatement()
        {
            // Arrange
            WithAPNCParser();
            WithALedgerService();

            // Act
            WhenACleanStatementIsImported();
            PTBRow ledger = WithTheFirstParsedLedger();

            // Assert
            ShouldParseFirstEntry(ledger);
        }
    }
}