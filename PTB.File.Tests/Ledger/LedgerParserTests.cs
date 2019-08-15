using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.File.Tests;

namespace PTB.File.Ledger.Tests
{
    [TestClass]
    public class TransactionParserTests : GlobalSetup
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
            var parser = new LedgerParser(Schema.Ledger);

            // Act
            line += System.Environment.NewLine;
            StringToLedgerResponse response = parser.ParseLine(line);
            Ledger result = response.Result;

            // Assert
            Assert.AreEqual(date, result.Date);
            Assert.AreEqual(type, result.Type);
            Assert.AreEqual(amount, result.Amount.TrimStart());
            Assert.AreEqual(subcategory, result.Subcategory.TrimStart());
            Assert.AreEqual(title, result.Title.TrimStart());
            Assert.AreEqual(location, result.Location.TrimStart());
            Assert.AreEqual(locked, result.Locked);
        }

        [TestMethod]
        public void FailsToParseLinesThatDoNotEndWithWindowsNewLine()
        {
            // Arrange
            string line = "2019-07-01 C        20.00                           directdeposittransferarfobkckwebxfrxxxxxx9349       000191709 1";
            var parser = new LedgerParser(Schema.Ledger);

            // Act
            StringToLedgerResponse response = parser.ParseLine(line);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Line does not end with carriage return, which may indicate data corruption", response.Message);
        }

        [TestMethod]
        public void FailsToParseLinesThatAreNotTheCorrectSize()
        {
            // Arrange
            string line = "2019-07-01 C       20.00                           directdeposittransferarfobkckwebxfrxxxxxx9349       000191709 1" + System.Environment.NewLine;
            var parser = new LedgerParser(Schema.Ledger);

            // Act
            StringToLedgerResponse response = parser.ParseLine(line);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Line length does not match schema, which may indicate data corruption.", response.Message);
        }
    }
}