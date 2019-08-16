using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTB.Core.Tests
{
    [TestClass]
    public class FileClientTests : GlobalSetup
    {
        [TestMethod]
        public void InstantiatesWithCleanConfigs()
        {
            // Arrange
            var fileManager = new FileManager(Settings, Schema);
            var client = new PTBClient();

            // Act
            client.Instantiate(fileManager);

            //Assert
            Assert.IsNotNull(client.Ledger);
            Assert.IsNotNull(client.Regex);
            Assert.IsNotNull(client.Budget);
            Assert.IsNotNull(client.Categories);
        }
    }
}