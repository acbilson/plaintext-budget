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
            var response = Client.Categories.ReadAllCategories();

            // Assert
            Assert.IsTrue(response.SkippedMessages.Count <= 0);
            Assert.AreEqual(39, response.Categories.Count);
        }
    }
}