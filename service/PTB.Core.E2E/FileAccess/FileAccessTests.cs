using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Files.FolderAccess;

namespace PTB.E2E.FileAccess
{
    [TestClass]
    [TestCategory("integration")]
    public class FileAccessTests : GlobalSetup
    {
        [TestMethod]
        [Description("Retrieves all ledger, categories, and title regex files")]
        public void GetsAllFiles()
        {
            // Arrange
            var fileFolderService = Provider.GetService<FileFolderService>();

            // Act
            var fileFolders = fileFolderService.GetFolders();

            // Assert - Gets correct file count
            Assert.AreEqual(3, fileFolders.LedgerFolder.Files.Count, $"Should have retrieved 3 ledger files, but retrieved {fileFolders.LedgerFolder.Files.Count}");
            Assert.AreEqual(1, fileFolders.TitleRegexFolder.Files.Count, $"Should have retrieved 1 title regex file, but retrieved {fileFolders.TitleRegexFolder.Files.Count}");

            // Assert - Gets default files (TBD)
        }
    }
}