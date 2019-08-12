using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PTB.File.Tests;
using System;

namespace PTB.File.Tests
{
    [TestClass]
    public class FileClientTests
    {
        [TestMethod]
        public void InstantiatesWithCleanConfigs()
        {
            // Arrange
            var client = new FileClient();

            // Act
            client.Instantiate(@".\Data\Clean");

            //Assert
            Assert.IsNotNull(client.Ledger);
            Assert.IsNotNull(client.Regex);
        }
    }
}
