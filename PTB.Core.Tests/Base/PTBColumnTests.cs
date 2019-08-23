using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.Base.Tests
{
    [TestClass]
    public class PTBColumnTests
    {
        [DataRow(30, "Groceries", "                     Groceries")]
        [DataRow(30, "   Groceries", "                     Groceries")]
        [DataRow(30, "Groceries   ", "                     Groceries")]
        [DataRow(12, "80.10", "       80.10")]
        [DataRow(5, "Trader Joe", "Trade")]
        [TestMethod]
        public void ParsesName(int size, string value, string expected)
        {
            // Arrange - Act
            var column = new PTBColumn
            {
                Size = size,
                ColumnValue = value
            };

            // Assert
            Assert.AreEqual(size, column.ColumnValue.Length);
            Assert.AreEqual(expected, column.ColumnValue);
        }
    }
}
