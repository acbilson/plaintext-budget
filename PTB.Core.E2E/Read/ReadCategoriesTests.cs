using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Categories;

namespace PTB.Core.E2E
{
    [TestClass]
    public class ReadCategories : GlobalSetup
    {
        [TestMethod]
        public void ReadAllCategories()
        {
            // Arrange
            WithAFileClient();

            // Act
            CategoriesReadResponse response = Client.Categories.ReadAllDefaultCategories();

            // Assert
            ShouldNotHaveAnySkippedCategories(response);
        }
    }
}