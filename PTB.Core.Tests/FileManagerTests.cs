using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.FileAccess;
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
            var fileManager = new BaseFileManager(Settings, Schema);
            var logger = new PTBFileLogger(LoggingLevel.Debug, Settings.HomeDirectory);
        }
    }
}