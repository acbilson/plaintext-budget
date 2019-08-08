using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Parser.Parsers;
using PTB.Parser.Objects;

namespace PTB.Parser.Tests
{
    [TestClass]
    public class PNCParserTests
    {
        [DataRow(@"2019/06/18,310.80,'Direct Deposit - Payroll','OPTIMUM JOY CLIN XXXXXXXXXXX39-0','000191699','CREDIT'")]
        [TestMethod]
        public void ParseCleanData(string one)
        {
            // Arrange
            var parser = new PNCParser();

            // Act
            PNCTransaction result = parser.Parse(one);

            // Assert
            Assert.AreEqual("2019-06-18", result.Date);

            Assert.AreEqual("310.80", result.Amount.TrimStart());
            Assert.AreEqual(12, result.Amount.Length);

            Assert.AreEqual("directdeposit-payrolloptimumjoyclinxxxxxxxxxxx39-0", result.Title);
            Assert.AreEqual(50, result.Title.Length);

            Assert.AreEqual("000191699", result.Location.TrimStart());
            Assert.AreEqual(15, result.Location.Length);
            Assert.AreEqual('C', result.Type);
        }
    }
}
