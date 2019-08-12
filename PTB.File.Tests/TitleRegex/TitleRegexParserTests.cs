using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PTB.File;
using PTB.File.Tests;
using PTB.File.TitleRegex;

namespace PTB.File.TitleRegex.Tests
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
            var parser = new TitleRegexParser(Schema.TitleRegex);

            // Act
            var result = parser.ParseLine(line);

            // Assert
            Assert.AreEqual(priority, result.Priority);
            Assert.AreEqual(subcategory, result.Subcategory.TrimStart());
            Assert.AreEqual(regex, result.Regex.TrimStart());
        }
    }
}