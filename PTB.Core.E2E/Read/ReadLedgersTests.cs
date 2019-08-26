using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTB.Core.E2E
{
    [TestClass]
    public class ReadLedgersTests : GlobalSetup
    {
        [TestMethod]
        public void ReadAllLedgerEntries()
        {
            // Arrange
            WithALedgerService();
            var defaultLedgerFile = FileFolders.LedgerFolder.GetDefaultFile();

            // Act
            var response = LedgerService.Read(defaultLedgerFile, 0, defaultLedgerFile.LineCount);

            // Assert
            Assert.AreEqual(30, response.ReadResult.Count);
        }

        [TestMethod]
        public void ReadFirstFiveLedgerEntries()
        {
            // Arrange
            int startIndex = 0;
            int ledgersToRead = 5;
            WithALedgerService();
            var defaultLedgerFile = FileFolders.LedgerFolder.GetDefaultFile();

            // Act
            var response = LedgerService.Read(defaultLedgerFile, startIndex, ledgersToRead);

            // Assert
            Assert.AreEqual(ledgersToRead, response.ReadResult.Count);
        }
    }
}