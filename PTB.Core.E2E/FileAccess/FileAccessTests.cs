using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTB.Core.E2E
{
    [TestClass]
    public class FileAccessTests : GlobalSetup
    {
        [TestInitialize]
        public void Initialize()
        {
            string folder = "FileAccess";
            GetDefaultSchema();
            GetDefaultSettings();
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
            var file = files.First(f => f.IsDefault);
            Assert.IsNotNull(file, "Should have identified the default categories file");
            Assert.AreEqual(Schema.Categories.DefaultFileName, System.IO.Path.GetFileNameWithoutExtension(file.Info.Name));
        }

        [TestMethod]
        public void GetsTitleRegexFile()
        {
            // Arrange
            WithAFileManager();

            // Act
            var file = FileManager.GetTitleRegexFile();

            // Assert
            Assert.AreEqual(Schema.TitleRegex.DefaultFileName, System.IO.Path.GetFileNameWithoutExtension(file.Name));
        }


    }
}
