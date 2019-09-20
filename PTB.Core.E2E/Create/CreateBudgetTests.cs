using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.E2E;
using PTB.Core.Reports;
using PTB.Reports.Budget;
using PTB.Reports.FolderAccess;
using System.Linq;

namespace PTB.Reports.E2E
{
    [TestClass]
    [TestCategory("integration")]
    public class CreateBudgetTests : GlobalSetup
    {
        [TestMethod]
        public void CreatesBudget()
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