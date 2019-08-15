﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTB.Core.E2E.PTBFileAccess
{
    [TestClass]
    public class FileAccessTests : GlobalSetup
    {
        [TestInitialize]
        public void Initialize()
        {
            string folder = "FileAccess";
            GetDefaultSchema(folder);
            GetDefaultSettings(folder);
        }

        [TestMethod]
        public void GetsThreeLedgerFiles()
        {
            // Arrange
            WithAFileManager();

            // Act
            var files = FileManager.GetLedgerFiles();

            // Assert
            Assert.AreEqual(3, files.Count());
        }

        [TestMethod]
        public void GetsDefaultLedgerFile()
        {
            // Arrange
            WithAFileManager();

            // Act
            var files = FileManager.GetLedgerFiles();

            // Assert
            var file = files.First(f => f.IsDefault);
            Assert.IsNotNull(file, "Should have identified the default ledger file");
            Assert.AreEqual(Schema.Ledger.DefaultFileName, System.IO.Path.GetFileNameWithoutExtension(file.Info.Name));
        }

    }
}
