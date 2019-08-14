using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PTB.File;
using PTB.File.Tests;
using PTB.File.Statements;

namespace PTB.File.Statements.Tests
{
    [TestClass]
    public class PNCParserTests : GlobalSetup
    {
        [DataRow("2019/06/18,310.80,'Direct Deposit - Payroll','OPTIMUM JOY CLIN XXXXXXXXXXX39-0','000191699','CREDIT'", "2019-06-18", "310.80", "directdepositpayrolloptimumjoyclinxxxxxxxxxxx390", "000191699", 'C')]
        [DataRow("2019/06/18,15.29,'5458 Debit Card Purchase Blaze Pizza Chicago Ev',,'4517145008','DEBIT'", "2019-06-18", "15.29", "5458debitcardpurchaseblazepizzachicagoev", "4517145008", 'D')]
        [DataRow("2019/06/18,11.80,'5458 Debit Card Purchase Andys Frozen Custard -',,'4517045008',\"DEBIT\"", "2019-06-18", "11.80", "5458debitcardpurchaseandysfrozencustard", "4517045008", 'D')]
        [DataRow("2019/06/18,73.67,'POS Purchase Trader Joe\'s Evanston Il',,'POS6947025','DEBIT'", "2019-06-18", "73.67", "pospurchasetraderjoesevanstonil", "pos6947025", 'D')]
        [DataRow("2019/06/18,69.95,'Web Pmt Single - Online Pmt Comcast Ckf244838860POS',,'000191699','DEBIT'", "2019-06-18", "69.95", "webpmtsingleonlinepmtcomcastckf244838860pos", "000191699", 'D')]
        [DataRow("2019/06/19,20.00,'Direct Deposit - Transfer','ARFOBK CK WEBXFR XXXXXX9349','000191709','CREDIT'", "2019-06-19", "20.00", "directdeposittransferarfobkckwebxfrxxxxxx9349", "000191709", 'C')]
        [DataRow("2019/06/19,4.25,'5458 Debit Card Purchase Metra Mobile',,'5018445008','DEBIT'", "2019-06-19", "4.25", "5458debitcardpurchasemetramobile", "5018445008", 'D')]
        [DataRow("2019/06/19,200.00,'ATM Withdrawal 1 N. Franklin Chicago Il',,'PNCPJ0268',\"DEBIT\"", "2019-06-19", "200.00", "atmwithdrawal1nfranklinchicagoil", "pncpj0268", 'D')]
        [DataRow("2019/06/20,3.85,'5458 Debit Card Purchase Starbucks Store 00233',,'8795545008','DEBIT'", "2019-06-20", "3.85", "5458debitcardpurchasestarbucksstore00233", "8795545008", 'D')]
        [DataRow("2019/07/01,9999.85,'5458-Debit/Card\\Purchase* Starbucks. `Store#00233',,'8795545008','DEBIT'", "2019-07-01", "9999.85", "5458debitcardpurchasestarbucksstore00233", "8795545008", 'D')]
        [TestMethod]
        public void ParseCleanData(string data, string date, string amount, string title, string location, char type)
        {
            // Arrange
            var parser = new PNCParser();

            // Act
            StatementParseResponse response = parser.ParseLine(data, Schema.Ledger);

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual(Schema.Ledger.Size, response.Result.Length);
        }

        [DataRow("00000000004604718986,2019/06/18,2019/07/16,7320.66,7763.23")]
        [TestMethod]
        public void SkipsSummaryColumn(string data)
        {
            // Arrange
            var parser = new PNCParser();

            // Act
            StatementParseResponse response = parser.ParseLine(data, Schema.Ledger);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Skip summary line", response.Message);
        }
    }
}