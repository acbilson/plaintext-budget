using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTB.Core.E2E
{
    [TestClass]
    public class ReadCategories : GlobalSetup
    {
        [TestMethod]
        public void ReadsDefaultCategories()
        {
            // Arrange
            WithACategoriesService();
            var defaultCategoriesFile = FileFolders.CategoriesFolder.GetDefaultFile();

            // Act
            var response = CategoriesService.Read(defaultCategoriesFile, 0, defaultCategoriesFile.LineCount);

            // Assert
            // TODO: must implement skipped messages
            //ShouldNotHaveAnySkippedCategories(response);
        }
    }
}