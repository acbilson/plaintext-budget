using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core;
using PTB.Core.Parsers;
using System;

namespace PTB.Core.Parsers.Tests
{
    [TestClass]
    public class PNCParserTests
    {
        [DataRow("2019/06/18,310.80,'Direct Deposit - Payroll','OPTIMUM JOY CLIN XXXXXXXXXXX39-0','000191699','CREDIT'", "2019-06-18", "310.80", "directdeposit-payrolloptimumjoyclinxxxxxxxxxxx39-0", "000191699", 'C')]
        [DataRow("2019/06/18,15.29,'5458 Debit Card Purchase Blaze Pizza Chicago Ev',,'4517145008','DEBIT'", "2019-06-18", "15.29", "5458debitcardpurchaseblazepizzachicagoev", "4517145008", 'D')]
        [DataRow("2019/06/18,11.80,'5458 Debit Card Purchase Andys Frozen Custard -',,'4517045008','DEBIT'", "2019-06-18", "11.80", "5458debitcardpurchaseandysfrozencustard-", "4517045008", 'D')]
        [DataRow("2019/06/18,73.67,'POS Purchase Trader Joe\'s Evanston Il',,'POS6947025','DEBIT'", "2019-06-18", "73.67", "pospurchasetraderjoesevanstonil", "pos6947025", 'D')]
        [DataRow("2019/06/18,69.95,'Web Pmt Single - Online Pmt Comcast Ckf244838860POS',,'000191699','DEBIT'", "2019-06-18", "69.95", "webpmtsingle-onlinepmtcomcastckf244838860pos", "000191699", 'D')]
        [DataRow("2019/06/19,20.00,'Direct Deposit - Transfer','ARFOBK CK WEBXFR XXXXXX9349','000191709','CREDIT'", "2019-06-19", "20.00", "directdeposit-transferarfobkckwebxfrxxxxxx9349", "000191709", 'C')]
        [DataRow("2019/06/19,4.25,'5458 Debit Card Purchase Metra Mobile',,'5018445008','DEBIT'", "2019-06-19", "4.25", "5458debitcardpurchasemetramobile", "5018445008", 'D')]
        [DataRow("2019/06/19,200.00,'ATM Withdrawal 1 N. Franklin Chicago Il',,'PNCPJ0268','DEBIT'", "2019-06-19", "200.00", "atmwithdrawal1n.franklinchicagoil", "pncpj0268", 'D')]
        [DataRow("2019/06/20,3.85,'5458 Debit Card Purchase Starbucks Store 00233',,'8795545008','DEBIT'", "2019-06-20", "3.85", "5458debitcardpurchasestarbucksstore00233", "8795545008", 'D')]
        [TestMethod]
        public void ParseCleanData(string data, string date, string amount, string title, string location, char type)
        {
            // Arrange
            var parser = new PNCParser();

            // Act
            Transaction result = parser.Parse(data);

            // Assert
            Assert.AreEqual(date, result.Date);
            Assert.AreEqual(ColumnSize.DATE, result.Date.Length);

            Assert.AreEqual(amount, result.Amount.TrimStart());
            Assert.AreEqual(ColumnSize.AMOUNT, result.Amount.Length);

            Assert.AreEqual(title, result.Title.TrimStart());
            Assert.AreEqual(ColumnSize.TITLE, result.Title.Length);

            Assert.AreEqual(location, result.Location.TrimStart());
            Assert.AreEqual(ColumnSize.LOCATION, result.Location.Length);

            Assert.AreEqual(type, result.Type);

            Assert.AreEqual('0', result.Locked);
            Assert.AreEqual(new String(' ', ColumnSize.SUBCATEGORY), result.Subcategory);
        }
    }
}
