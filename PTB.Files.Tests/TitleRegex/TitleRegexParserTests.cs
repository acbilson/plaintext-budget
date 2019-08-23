using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Tests;

namespace PTB.Core.TitleRegex.Tests
{
    [TestClass]
    public class TitleRegexParserTests : GlobalSetup
    {
        /*

        [DataRow("1                         Coffee            BROTHERSK                     Brothers K", '1', "Coffee", "BROTHERSK", "Brothers K")]
        [DataRow("1                         Coffee            STARBUCKS                      Starbucks", '1', "Coffee", "STARBUCKS", "Starbucks")]
        [DataRow("1                      Groceries                JEWEL                          Jewel", '1', "Groceries", "JEWEL", "Jewel")]
        [DataRow("2                      Groceries          TRADER.*JOE                   Trader Joe's", '2', "Groceries", "TRADER.*JOE", "Trader Joe's")]
        [TestMethod]
        public void ParsesCleanData(string line, char priority, string subcategory, string regex, string subject)
        {
            // Arrange
            line += System.Environment.NewLine;
            var parser = new TitleRegexParser(Schema);

            // Act
            var response = parser.ParseLine(line);
            var result = response.Result;

            // Assert
            Assert.IsTrue(response.Success, $"Title regex parsing failed with this message: {response.Message}");
            Assert.AreEqual(priority, result.Priority);
            Assert.AreEqual(subcategory, result.Subcategory.TrimStart());
            Assert.AreEqual(regex, result.Regex.TrimStart());
            Assert.AreEqual(subject, result.Subject.TrimStart());
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
        */
    }
}