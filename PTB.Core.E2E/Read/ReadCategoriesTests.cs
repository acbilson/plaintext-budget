using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTB.Core.E2E
{
    [TestClass]
    public class ReadCategories : GlobalSetup
    {
        [TestMethod]
        public void ReadAllCategories()
        {
            // Arrange
            WithAFileClient();

            // Act
            var response = Client.Categories.ReadAllDefaultCategories();

            // Assert
            if (response.SkippedMessages.Count > 0)
            {
                string messages = string.Join(' ', response.SkippedMessages);
                Assert.Fail($"There were skipped ledgers while categorizing. Messages: {messages}");
            }
            Assert.AreEqual(39, response.Categories.Count);
        }
    }
}