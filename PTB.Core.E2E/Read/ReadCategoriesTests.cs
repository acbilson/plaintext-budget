using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Reports.Categories;
using PTB.Reports.FolderAccess;

namespace PTB.Core.E2E
{
    [TestClass]
    [TestCategory("integration")]
    public class ReadCategories : GlobalSetup
    {
        [TestMethod]
        public void ReadsDefaultCategories()
        {
            // Arrange
            var categoriesService = Provider.GetService<CategoriesService>();
            var defaultCategoriesFile = ReportFolders.CategoriesFolder.GetDefaultFile();

            // Act
            var response = categoriesService.Read(defaultCategoriesFile, 0, defaultCategoriesFile.LineCount);

            // Assert
            // TODO: must implement skipped messages
            //ShouldNotHaveAnySkippedCategories(response);
        }
    }
}