using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Tests;

namespace PTB.Core.TitleRegex.Tests
{
    [TestClass]
    public class TitleRegexParserTests : GlobalSetup
    {
        [DataRow("1                      Groceries                          JEWEL", '1', "Groceries", "JEWEL")]
        [DataRow("1                      Groceries                     TRADER JOE", '1', "Groceries", "TRADER JOE")]
        [DataRow("1                         Coffee                     BROTHERS K", '1', "Coffee", "BROTHERS K")]
        [DataRow("1                         Coffee                   OTHERBROTHER", '1', "Coffee", "OTHERBROTHER")]
        [DataRow("1                         Coffee                      STARBUCKS", '1', "Coffee", "STARBUCKS")]
        [DataRow("2                         Target                         TARGET", '2', "Target", "TARGET")]
        [TestMethod]
        public void ParsesCleanData(string line, char priority, string subcategory, string regex)
        {
            // Arrange
            line += System.Environment.NewLine;
            var parser = new TitleRegexParser(Schema.TitleRegex);

            // Act
            var response = parser.ParseLine(line);
            var result = response.Result;

            // Assert
            Assert.AreEqual(priority, result.Priority);
            Assert.AreEqual(subcategory, result.Subcategory.TrimStart());
            Assert.AreEqual(regex, result.Regex.TrimStart());
        }

        [TestMethod]
        public void FailsToParseLinesThatDoNotEndWithWindowsNewLine()
        {
            // Arrange
            string line = "1                      Groceries                          JEWEL";
            var parser = new TitleRegexParser(Schema.TitleRegex);

            // Act
            var response = parser.ParseLine(line);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Line does not end with carriage return, which may indicate data corruption", response.Message);
        }

        [TestMethod]
        public void FailsToParseLinesThatAreNotTheCorrectSize()
        {
            // Arrange
            string line = "1                     Groceries                          JEWEL" + System.Environment.NewLine;
            var parser = new TitleRegexParser(Schema.TitleRegex);

            // Act
            var response = parser.ParseLine(line);

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Line length does not match schema, which may indicate data corruption.", response.Message);
        }
    }
}