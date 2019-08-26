using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.FolderAccess;
using PTB.Files.FolderAccess;

namespace PTB.Core.E2E
{
    [TestClass]
    public class FileAccessTests : GlobalSetup
    {
        [TestMethod, Description("Retrieves all ledger, categories, and title regex files")]
        public void GetsAllFiles()
        {
            // Arrange
            var fileFolderService = Provider.GetService<FileFolderService>();

            // Act
            var fileFolders = fileFolderService.GetFileFolders();

            // Assert - Gets correct file count
            Assert.AreEqual(3, fileFolders.LedgerFolder.Files.Count, $"Should have retrieved 3 ledger files, but retrieved {fileFolders.LedgerFolder.Files.Count}");
            Assert.AreEqual(2, fileFolders.CategoriesFolder.Files.Count, $"Should have retrieved 2 categories files, but retrieved {fileFolders.CategoriesFolder.Files.Count}");
            Assert.AreEqual(1, fileFolders.TitleRegexFolder.Files.Count, $"Should have retrieved 1 title regex file, but retrieved {fileFolders.TitleRegexFolder.Files.Count}");

            // Assert - Gets default files (TBD)
        }
    }
}
