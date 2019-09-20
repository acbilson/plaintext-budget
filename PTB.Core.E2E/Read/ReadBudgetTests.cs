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
    public class ReadBudgetTests : GlobalSetup
    {
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
            ShouldReadSubcategoryValues(actual);
        }

        public void ShouldReadSubcategoryValues(ReportReadResponse actual)
        {
            string amazonCategory = "Amazon Membership";
            string amazonAmount = "50.00";
            var amazonRow = actual.GetRowBySubcategoryValue(amazonCategory);
            Assert.IsNotNull(amazonRow, $"Should have row for {amazonCategory}");
            Assert.AreEqual(amazonAmount, amazonRow["Amount"].TrimEnd(), $"Should have set {amazonCategory} budget to {amazonAmount}");
        }
    }
}