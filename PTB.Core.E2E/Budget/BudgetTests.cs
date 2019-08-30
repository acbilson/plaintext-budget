using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.E2E;
using PTB.Reports.Budget;
using PTB.Reports.FolderAccess;

namespace PTB.Reports.E2E
{
    [TestClass]
    public class BudgetTests : GlobalSetup
    {
        [TestMethod]
        public void GeneratesBudget()
        {
            // Arrange
            var budgetService = Provider.GetService<BudgetService>();
            var budgetFolderService = Provider.GetService<BudgetFolderService>();
            var budgetFile = budgetFolderService.CreateNewBudgetFile();
            var categories = WithAllCategories();

            // Act
            budgetService.Create(budgetFile, categories);

            // Assert
            string[] lines = WithAllBudgetLines(budgetFile.FullPath);
            ShouldGenerateABudgetOfTheRightSize(lines);
            ShouldGenerateASortedBudget(lines);
        }
    }
}