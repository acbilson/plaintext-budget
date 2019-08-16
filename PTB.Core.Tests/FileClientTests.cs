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
            var client = new PTBClient();

            // Act
            client.Instantiate(Settings);

            //Assert
            Assert.IsNotNull(client.Ledger);
            Assert.IsNotNull(client.Regex);
            Assert.IsNotNull(client.Budget);
            Assert.IsNotNull(client.Categories);
        }
    }
}