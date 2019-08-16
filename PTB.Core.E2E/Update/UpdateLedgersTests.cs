using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PTB.Core.E2E
{
    [TestClass]
    public class UpdateLedgersTests : GlobalSetup
    {
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