using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.File.E2E
{
    [TestClass]
    public class ImportStatementTests : GlobalSetup
    {

        [TestInitialize]
        public void Initialize()
        {
            string folder = "Import";
            GetDefaultSchema(folder);
            GetDefaultSettings(folder);
        }

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
            Ledger.Ledger ledger = WithAParsedLedger();

            // Assert
            ShouldParseFirstEntry(ledger);
        }

    }
}
