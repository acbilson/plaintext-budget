using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.File.E2E
{
    [TestClass]
    public class UpdateLedgers : GlobalSetup
    {
        [TestInitialize]
        public void Initialize()
        {
            string folder = "Update";
            GetDefaultSchema(folder);
            GetDefaultSettings(folder);
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
