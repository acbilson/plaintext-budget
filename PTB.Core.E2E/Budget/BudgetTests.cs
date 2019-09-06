using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.E2E;
using PTB.Reports.Budget;
using PTB.Reports.FolderAccess;
using System.Linq;

namespace PTB.Reports.E2E
{
    [TestClass]
    public class BudgetTests : GlobalSetup
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

        [TestMethod]
        public void ReadsBudget()
        {
            // Arrange
            BudgetFile budgetFile = ReportFolders.BudgetFolder.Files.First(file => file.StartDate == new System.DateTime(2018, 4, 1));
            var budgetService = Provider.GetService<BudgetService>();

            // Act
            var actual = budgetService.Read(budgetFile, 0, budgetFile.LineCount);

            // Assert
            Assert.IsTrue(actual.Success);
        }
    }
}