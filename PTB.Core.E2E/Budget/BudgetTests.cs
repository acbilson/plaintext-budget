using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace PTB.Core.E2E
{
    [TestClass]
    public class BudgetTests : GlobalSetup
    {
        [TestMethod]
        public void GeneratesBudget()
        {
            // Arrange
            var categories = WithAllCategories();

            // Act
            //Client.Budget.CreateBudget(categories);

            // Assert
            //string[] lines = WithBudgetLines();
            //ShouldGenerateABudgetOfTheRightSize(lines);
            //ShouldGenerateASortedBudget(lines);
        }
    }
}