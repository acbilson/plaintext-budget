using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PTB.Core.E2E
{
    [TestClass]
    public class UpdateLedgersTests : GlobalSetup
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
        public void ShouldUpdateFourthLedgerEntry()
        {
            // Arrange
            WithAFileClient();
            WithALedgerParser();
            var ledgerToUpdate = WithTheFourthLedger();
            string newSubcategory = "TestCategory";
            ledgerToUpdate.Subcategory = new String(' ', Schema.Ledger.Columns.Subcategory.Size - newSubcategory.Length) + newSubcategory;

            // Act
            var response = WhenALedgerIsUpdated(ledgerToUpdate);

            // Assert
            Assert.IsTrue(response.Success);
            ShouldUpdateFourthEntryWithNewSubcategory(ledgerToUpdate.Subcategory);
        }
    }
}