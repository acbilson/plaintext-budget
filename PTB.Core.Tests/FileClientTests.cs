using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTB.Core.Tests
{
    [TestClass]
    public class FileClientTests
    {
        [TestMethod]
        public void InstantiatesWithCleanConfigs()
        {
            // Arrange
            var client = new PTBClient();

            // Act
            client.Instantiate(@".\Config\Clean");

            //Assert
            Assert.IsNotNull(client.Ledger);
            Assert.IsNotNull(client.Regex);
            Assert.IsNotNull(client.Budget);
            Assert.IsNotNull(client.Categories);
        }
    }
}