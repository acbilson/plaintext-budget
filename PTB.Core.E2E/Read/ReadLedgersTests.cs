using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Files.Ledger;

namespace PTB.Core.E2E
{
    [TestClass]
    public class ReadLedgersTests : GlobalSetup
    {
        [TestMethod]
        public void ReadAllLedgerEntries()
        {
            // Arrange
            var ledgerService = Provider.GetService<LedgerService>();
            var defaultLedgerFile = FileFolders.LedgerFolder.GetDefaultFile();

            // Act
            var response = ledgerService.Read(defaultLedgerFile, 0, defaultLedgerFile.LineCount);

            // Assert
            Assert.AreEqual(30, response.ReadResult.Count);
        }

        [TestMethod]
        public void ReadFirstFiveLedgerEntries()
        {
            // Arrange
            int startIndex = 0;
            int ledgersToRead = 5;
            var ledgerService = Provider.GetService<LedgerService>();
            var defaultLedgerFile = FileFolders.LedgerFolder.GetDefaultFile();

            // Act
            var response = ledgerService.Read(defaultLedgerFile, startIndex, ledgersToRead);

            // Assert
            Assert.AreEqual(ledgersToRead, response.ReadResult.Count);
        }
    }
}