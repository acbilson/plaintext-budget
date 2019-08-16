using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTB.Core.E2E
{
    [TestClass]
    public class ImportStatementTests : GlobalSetup
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