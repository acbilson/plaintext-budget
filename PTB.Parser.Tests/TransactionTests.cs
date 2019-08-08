using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core;
using PTB.Core.Parsers;

namespace PTB.Core.Tests
{
    [TestClass]
    public class TransactionTests
    {
        [TestMethod]
        public void PrintCleanString()
        {
            // Arrange
            string expected = "2019-06-18       310.80                      C 0 directdeposit-payrolloptimumjoyclinxxxxxxxxxxx39-0       000191699";

            int delimiterSpace = 6;
            int expectedLength = ColumnSize.DATE + ColumnSize.AMOUNT + ColumnSize.SUBCATEGORY + ColumnSize.TYPE + ColumnSize.LOCKED + ColumnSize.TITLE + ColumnSize.LOCATION + delimiterSpace;

            // Act
            string actual = new Transaction("2019-06-18", "      310.80", "directdeposit-payrolloptimumjoyclinxxxxxxxxxxx39-0", "      000191699", 'C').ToString();

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedLength, actual.Length);
        }
    }
}
