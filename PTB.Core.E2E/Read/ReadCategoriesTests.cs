using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTB.Core.E2E
{
    [TestClass]
    public class ReadCategories : GlobalSetup
    {
        [TestInitialize]
        public void Initialize()
        {
            string folder = "Read";
            GetDefaultSchema(folder);
            GetDefaultSettings(folder);
        }

        [TestMethod]
        public void ReadAllCategories()
        {
            // Arrange
            WithAFileClient();

            // Act
            var response = Client.Categories.ReadAllCategories();

            // Assert
            Assert.IsTrue(response.SkippedMessages.Count <= 0);
            Assert.AreEqual(40, response.Categories.Count);
        }
    }
}