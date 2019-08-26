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
            WithALedgerFileParser();
            WithALedgerService();
            var ledgerToUpdate = WithTheFourthLedger();
            string newSubcategory = "TestCategory";
            ledgerToUpdate["subcategory"] = new String(' ', Schema.Ledger["subcategory"].Size - newSubcategory.Length) + newSubcategory;

            // Act
            var response = WhenALedgerIsUpdated(ledgerToUpdate.Index, ledgerToUpdate);

            // Assert
            Assert.IsTrue(response.Success);
            ShouldUpdateFourthEntryWithNewSubcategory(ledgerToUpdate["subcategory"]);
        }
    }
}