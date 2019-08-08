using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core;
using PTB.Core.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

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
            var parser = new TitleRegexParser();

            // Act
            var result = parser.Parse(line);

            // Assert
            Assert.AreEqual(priority, result.Priority);
            Assert.AreEqual(subcategory, result.Subcategory.TrimStart());
            Assert.AreEqual(regex, result.Regex.TrimStart());
        }







    }
}
