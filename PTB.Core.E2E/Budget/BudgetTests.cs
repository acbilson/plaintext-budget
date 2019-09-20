using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using PTB.Core.E2E;
using PTB.Core.Reports;
using PTB.Reports.Budget;
using PTB.Reports.FolderAccess;
using System.Linq;

namespace PTB.Reports.E2E
{
    [TestClass]
    [TestCategory("integration")]
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
        public void ReadsAndParsesBudget()
        {
            // Arrange
            BudgetFile budgetFile = ReportFolders.BudgetFolder.Files.First(file => file.StartDate == new System.DateTime(2018, 4, 1));
            var budgetService = Provider.GetService<BudgetService>();

            // Act
            var actual = budgetService.Read(budgetFile, 0, budgetFile.LineCount);

            // Assert
            Assert.IsTrue(actual.Success);
            Assert.AreEqual(1, actual.ReadResult[0].Columns.Count, "Should have a single column for section header row");
            Assert.AreEqual(2, actual.ReadResult[1].Columns.Count, "Should have two columns for subcategory row");

            var amazonRow = actual.GetRowBySubcategoryValue("Amazon Membership");
            Assert.IsNotNull(amazonRow, "Should have row for Amazon Membership");
            Assert.AreEqual("50.00", amazonRow["Amount"].TrimEnd(), "Should have set Amazon Membership budget to 50.00");
        }
    }
}