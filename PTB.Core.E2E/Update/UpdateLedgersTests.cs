using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PTB.Core.E2E
{
    [TestClass]
    public class UpdateLedgersTests : GlobalSetup
    {
        [TestMethod]
        public void ShouldUpdateFourthLedgerEntry()
        {
            // Arrange
            var ledgerToUpdate = WithTheFourthLedger();
            string newSubcategory = "TestCategory";
            ledgerToUpdate["subcategory"] = newSubcategory;

            // Act
            var response = WhenALedgerIsUpdated(ledgerToUpdate.Index, ledgerToUpdate);

            // Assert
            Assert.IsTrue(response.Success);
            ShouldUpdateFourthEntryWithNewSubcategory(ledgerToUpdate["subcategory"]);
        }

        [TestMethod]
        public void ShouldNotUpdateUneditableColumn()
        {
            // Arrange
            var ledgerToUpdate = WithTheFourthLedger();
            string newAmount = "999.99";
            ledgerToUpdate["amount"] = newAmount;

            // Act
            var response = WhenALedgerIsUpdated(ledgerToUpdate.Index, ledgerToUpdate);

            // Assert
            Assert.IsTrue(response.Success);
            ShouldNotUpdateFourthEntryWithNewAmount(ledgerToUpdate["amount"]);
        }

    }
}