using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PTB.File;
using PTB.File.TitleRegex;

namespace PTB.Core.Tests.Parsers
{
    [TestClass]
    public class TitleRegexParserTests
    {
        [DataRow("1                      Groceries                          JEWEL", '1', "Groceries", "JEWEL")]
        [DataRow("1                      Groceries                     TRADER JOE", '1', "Groceries", "TRADER JOE")]
        [DataRow("1                         Coffee                     BROTHERS K", '1', "Coffee", "BROTHERS K")]
        [DataRow("1                         Coffee                   OTHERBROTHER", '1', "Coffee", "OTHERBROTHER")]
        [DataRow("1                         Coffee                      STARBUCKS", '1', "Coffee", "STARBUCKS")]
        [DataRow("2                         Target                         TARGET", '2', "Target", "TARGET")]
        [TestMethod]
        public void Test(string line, char priority, string subcategory, string regex)
        {
            // Arrange
            string text = System.IO.File.ReadAllText(@"Data\clean-schema.json");
            PTBSchema schema = JsonConvert.DeserializeObject<PTBSchema>(text);
            var parser = new TitleRegexParser(schema.Regex);

            // Act
            var result = parser.ParseLine(line);

            // Assert
            Assert.AreEqual(priority, result.Priority);
            Assert.AreEqual(subcategory, result.Subcategory.TrimStart());
            Assert.AreEqual(regex, result.Regex.TrimStart());
        }
    }
}