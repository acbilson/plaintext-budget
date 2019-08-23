﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.FileAccess;

namespace PTB.Core.E2E
{
    [TestClass]
    public class FileAccessTests : GlobalSetup
    {

        [TestMethod]
        public void ReadsConfigsFromPath()
        {
            // Arrange - Act
            var fileManager = new BaseFileManager(CleanSettings, Schema);
        }

        [TestMethod, Description("There are four files present in the Ledgers folder, but one does not match the ledger file mask")]
        public void GetsOnlyThreeLedgerFiles()
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
            BasePTBFile file = files.First(f => f.IsDefault);
            Assert.IsNotNull(file, "Should have identified the default ledger file");
            Assert.AreEqual(Schema.Ledger.DefaultFileName, System.IO.Path.GetFileNameWithoutExtension(file.FullPath));
        }

        [TestMethod]
        public void GetsTwoCategoriesFiles()
        {
            // Arrange
            WithAFileManager();

            // Act
            var files = FileManager.GetCategoriesFiles();

            // Assert
            Assert.AreEqual(2, files.Count());
        }

        [TestMethod]
        public void GetsDefaultCategoriesFile()
        {
            // Arrange
            WithAFileManager();

            // Act
            var files = FileManager.GetCategoriesFiles();

            // Assert
            BasePTBFile file = files.First(f => f.IsDefault);
            Assert.IsNotNull(file, "Should have identified the default categories file");
            Assert.AreEqual(Schema.Categories.DefaultFileName, System.IO.Path.GetFileNameWithoutExtension(file.FullPath));
        }

        [TestMethod]
        public void GetsTitleRegexFile()
        {
            // Arrange
            WithAFileManager();

            // Act
            string filePath = FileManager.GetTitleRegexFilePath();

            // Assert
            Assert.AreEqual(Schema.TitleRegex.DefaultFileName, System.IO.Path.GetFileNameWithoutExtension(filePath));
        }
    }
}
