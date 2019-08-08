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
            string expected = "2019-06-18 C       310.80                        directdepositpayrolloptimumjoyclinxxxxxxxxxxx390       000191699 0";

            int delimiterSpace = 6;
            int expectedLength = TransactionColumnSize.DATE + TransactionColumnSize.AMOUNT + TransactionColumnSize.SUBCATEGORY + TransactionColumnSize.TYPE + TransactionColumnSize.LOCKED + TransactionColumnSize.TITLE + TransactionColumnSize.LOCATION + delimiterSpace;

            // Act
            string actual = new Transaction("2019-06-18", "      310.80", "  directdepositpayrolloptimumjoyclinxxxxxxxxxxx390", "      000191699", 'C').ToString();

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedLength, actual.Length);
            Assert.AreEqual(expectedLength, Constant.TRANSACTION_SIZE);
        }
    }
}
