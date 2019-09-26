using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using PTB.Core.Files;
using PTB.Core.Tests;

namespace PTB.Core.Ledger.Tests
{
    [TestClass]
    public class LedgerParserTests : GlobalSetup
    {
        [DataRow("2019-06-18 C       310.80                                                                 directdepositpayrolloptimumjoyclinxxxxxxxxxxx390 0", "2019-06-18", "C", "310.80", "", "directdepositpayrolloptimumjoyclinxxxxxxxxxxx390", "", "0")]
        [DataRow("2019-06-18 D        11.80                                                                          5458debitcardpurchaseandysfrozencustard 0", "2019-06-18", "D", "11.80", "", "5458debitcardpurchaseandysfrozencustard", "", "0")]
        [DataRow("2019-06-18 D        15.29                                                                         5458debitcardpurchaseblazepizzachicagoev 0", "2019-06-18", "D", "15.29", "", "5458debitcardpurchaseblazepizzachicagoev", "", "0")]
        [DataRow("2019-06-18 D        69.95                                                                      webpmtsingleonlinepmtcomcastckf244838860pos 0", "2019-06-18", "D", "69.95", "", "webpmtsingleonlinepmtcomcastckf244838860pos", "", "0")]
        [DataRow("2019-06-18 D        73.67                                                                                  pospurchasetraderjoesevanstonil 0", "2019-06-18", "D", "73.67", "", "pospurchasetraderjoesevanstonil", "", "0")]
        [DataRow("2019-06-19 C        20.00                                                                    directdeposittransferarfobkckwebxfrxxxxxx9349 0", "2019-06-19", "C", "20.00", "", "directdeposittransferarfobkckwebxfrxxxxxx9349", "", "0")]
        [DataRow("2019-07-01 C        20.00                                                                    directdeposittransferarfobkckwebxfrxxxxxx9349 1", "2019-07-01", "C", "20.00", "", "directdeposittransferarfobkckwebxfrxxxxxx9349", "", "1")]
        [TestMethod]
        public void ParsesCleanLines(string line, string date, string type, string amount, string subcategory, string title, string subject, string locked)
        {
            // Arrange
            var parser = new BaseFileParser(Schema.Ledger, MockLogger.Object, new FileValidation(MockLogger.Object));

            // Act
            line += System.Environment.NewLine;
            StringToRowResponse response = parser.ParseLine(line, 0);
            var row = response.Row;

            // Assert
            Assert.IsTrue(response.Success, $"Failed to parse ledger with message: {response.Message}");

            Assert.AreEqual(date, row["date"]);
            Assert.AreEqual(type, row["type"]);
            Assert.AreEqual(amount, row["amount"].TrimStart());
            Assert.AreEqual(subcategory, row["subcategory"].TrimStart());
            Assert.AreEqual(title, row["title"].TrimStart());
            Assert.AreEqual(subject, row["subject"].TrimStart());
            Assert.AreEqual(locked, row["locked"]);
        }

        [TestMethod]
        public void FailsToParseLinesThatDoNotEndWithWindowsNewLine()
        {
            // Arrange
            string line = "2019-07-01 C        20.00                           directdeposittransferarfobkckwebxfrxxxxxx9349       000191709 1";
            var parser = new BaseFileParser(Schema.Ledger, MockLogger.Object, new FileValidation(MockLogger.Object));

            // Act
            StringToRowResponse response = parser.ParseLine(line, 0);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual(ParseMessages.LINE_NO_CR, response.Message);
        }

        [TestMethod]
        public void FailsToParseLinesThatAreNotTheCorrectSize()
        {
            // Arrange
            string line = "2019-07-01 C       20.00                           directdeposittransferarfobkckwebxfrxxxxxx9349       000191709 1" + System.Environment.NewLine;
            var parser = new BaseFileParser(Schema.Ledger, MockLogger.Object, new FileValidation(MockLogger.Object));

            // Act
            StringToRowResponse response = parser.ParseLine(line, 0);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual(ParseMessages.LINE_LENGTH_MISMATCH_SCHEMA, response.Message);
        }
    }
}