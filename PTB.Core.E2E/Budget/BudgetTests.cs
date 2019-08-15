using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace PTB.Core.E2E
{
    [TestClass]
    public class BudgetTests : GlobalSetup
    {
        [TestInitialize]
        public void Initialize()
        {
            string folder = "Budget";
            GetDefaultSchema(folder);
            GetDefaultSettings(folder);
        }

        [TestMethod]
        public void GeneratesBudget()
        {
            // Arrange
            WithAFileClient();
            var categories = WithAllCategories();

            // Act
            Client.Budget.CreateBudget(categories);

            // Assert
            string[] lines = WithBudgetLines();
            ShouldGenerateABudgetOfTheRightSize(lines);
            ShouldGenerateASortedBudget(lines);
        }
    }
}