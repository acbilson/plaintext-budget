using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Logging;

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
            var logger = PTBFileLogger.Instance;
            logger.Configure(LoggingLevel.Debug, Settings.HomeDirectory);
            var client = PTBClient.Instance;

            // Act
            client.Instantiate(fileManager, logger);

            //Assert
            Assert.IsNotNull(client.Ledger);
            Assert.IsNotNull(client.Regex);
            Assert.IsNotNull(client.Budget);
            Assert.IsNotNull(client.Categories);
        }
    }
}