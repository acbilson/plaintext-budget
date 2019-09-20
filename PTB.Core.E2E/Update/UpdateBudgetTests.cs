using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace PTB.Core.E2E
{
    [TestClass]
    [TestCategory("integration")]
    public class UpdateBudgetTests : GlobalSetup
    {
        [TestMethod]
        public void ShouldUpdateBudgetRecord()
        {
            // Arrange
            int elementIndex = 4;
            var records = WithAllBudgetRecords();
            var recordToUpdate = records.ElementAt(elementIndex);
            recordToUpdate["Amount"] = "30.00";

            // Act
            var response = WhenABudgetIsUpdated(recordToUpdate.Index, recordToUpdate);

            // Assert
            Assert.IsTrue(response.Success);
            records = WithAllBudgetRecords();
            var updatedRecord = records.ElementAt(elementIndex);
            Assert.AreEqual(recordToUpdate["Amount"], updatedRecord["Amount"]);
        }

        [TestMethod]
        [Ignore]
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