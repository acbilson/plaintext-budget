using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTB.Core.E2E
{
    [TestClass]
    public class ReadLedgersTests : GlobalSetup
    {
        [TestInitialize]
        public void Initialize()
        {
            string folder = "Read";
            GetDefaultSchema(folder);
            GetDefaultSettings(folder);
        }

        [TestMethod]
        public void ReadAllLedgerEntries()
        {
            // Arrange
            WithAFileClient();

            // Act
            var result = Client.Ledger.ReadDefaultLedgerEntries(0, 10000);

            // Assert
            Assert.AreEqual(30, result.Count);
        }

        [TestMethod]
        public void ReadFirstFiveLedgerEntries()
        {
            // Arrange
            int startIndex = 0;
            int ledgersToRead = 5;
            WithAFileClient();

            // Act
            var result = Client.Ledger.ReadDefaultLedgerEntries(startIndex, ledgersToRead);

            // Assert
            Assert.AreEqual(ledgersToRead, result.Count);
        }
    }
}