using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Files.Categories;

namespace PTB.Core.E2E
{
    [TestClass]
    public class ReadCategories : GlobalSetup
    {
        [TestMethod]
        public void ReadsDefaultCategories()
        {
            // Arrange
            var categoriesService = Provider.GetService<CategoriesService>();
            var defaultCategoriesFile = FileFolders.CategoriesFolder.GetDefaultFile();

            // Act
            var response = categoriesService.Read(defaultCategoriesFile, 0, defaultCategoriesFile.LineCount);

            // Assert
            // TODO: must implement skipped messages
            //ShouldNotHaveAnySkippedCategories(response);
        }
    }
}