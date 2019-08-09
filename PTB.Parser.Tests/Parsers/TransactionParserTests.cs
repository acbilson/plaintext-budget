using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.Parsers.Tests
{
    [TestClass]
    public class TransactionParserTests
    {
        [DataRow("2019-06-18 C       310.80                        directdepositpayrolloptimumjoyclinxxxxxxxxxxx390       000191699 0", "2019-06-18", 'C', "310.80", "", "directdepositpayrolloptimumjoyclinxxxxxxxxxxx390", "000191699", '0')]
        [DataRow("2019-06-18 D        11.80                                 5458debitcardpurchaseandysfrozencustard      4517045008 0", "2019-06-18", 'D', "11.80", "", "5458debitcardpurchaseandysfrozencustard", "4517045008", '0')]
        [DataRow("2019-06-18 D        15.29                                5458debitcardpurchaseblazepizzachicagoev      4517145008 0", "2019-06-18", 'D', "15.29", "", "5458debitcardpurchaseblazepizzachicagoev", "4517145008", '0')]
        [DataRow("2019-06-18 D        69.95                             webpmtsingleonlinepmtcomcastckf244838860pos       000191699 0", "2019-06-18", 'D', "69.95", "", "webpmtsingleonlinepmtcomcastckf244838860pos", "000191699", '0')]
        [DataRow("2019-06-18 D        73.67                                         pospurchasetraderjoesevanstonil      pos6947025 0", "2019-06-18", 'D', "73.67", "", "pospurchasetraderjoesevanstonil", "pos6947025", '0')]
        [DataRow("2019-06-19 C        20.00                           directdeposittransferarfobkckwebxfrxxxxxx9349       000191709 0", "2019-06-19", 'C', "20.00", "", "directdeposittransferarfobkckwebxfrxxxxxx9349", "000191709", '0')]
        [DataRow("2019-07-01 C        20.00                           directdeposittransferarfobkckwebxfrxxxxxx9349       000191709 1", "2019-07-01", 'C', "20.00", "", "directdeposittransferarfobkckwebxfrxxxxxx9349", "000191709", '1')]
        [TestMethod]
        public void ParsesCleanLines(string line, string date, char type, string amount, string subcategory, string title, string location, char locked)
        {
            // Arrange
            var parser = new TransactionParser();

            // Act
            Transaction result = parser.ParseLine(line);

            // Assert
            Assert.AreEqual(date, result.Date);
            Assert.AreEqual(type, result.Type);
            Assert.AreEqual(amount, result.Amount.TrimStart());
            Assert.AreEqual(subcategory, result.Subcategory.TrimStart());
            Assert.AreEqual(title, result.Title.TrimStart());
            Assert.AreEqual(location, result.Location.TrimStart());
            Assert.AreEqual(locked, result.Locked);
        }
    }
}
