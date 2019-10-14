using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Reports.Tests;

namespace PTB.Reports.FolderAccess.Tests
{
    [TestClass]
    public class BudgetFileTests : GlobalSetup
    {
        [DataRow("budget_19-01-01_to_31.txt", "19-01-01", "19-01-31")]
        [TestMethod]
        public void ParsesFileName(string fileName, string startDate, string endDate)
        {
            // Arrange
            var expectedStartDate = DateTime.ParseExact(startDate, "yy-MM-dd", CultureInfo.InvariantCulture);
            var expectedEndDate = DateTime.ParseExact(endDate, "yy-MM-dd", CultureInfo.InvariantCulture);
            var path = System.IO.Path.Combine(Settings.HomeDirectory, "Budgets", fileName);

            // Act
            var actual = new BudgetFile(Settings.FileDelimiter, Schema.Budget.LineSize, new System.IO.FileInfo(path));

            // Assert
            Assert.AreEqual(expectedStartDate, actual.StartDate);
            Assert.AreEqual(expectedEndDate, actual.EndDate);
        }
    }
}
