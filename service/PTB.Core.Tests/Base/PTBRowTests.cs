using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using PTB.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.Base.Tests
{
    [TestClass]
    public class PTBRowTests
    {
        private PTBRow GetDefaultRow()
        {
            return new PTBRow
            {
                Columns = new List<PTBColumn>
                {
                    new PTBColumn
                    {
                        ColumnName = "Amount",
                        Size = 12,
                        ColumnValue = "80.20"
                    },
                    new PTBColumn
                    {
                        ColumnName = "Subcategory",
                        Size = 30,
                        ColumnValue = "Groceries"
                    },
                    new PTBColumn
                    {
                        ColumnName = "Locked",
                        Size = 1,
                        ColumnValue = "0"
                    }

                }
            };
        }

        [DataRow("amount", "80.20")]
        [DataRow("subcategory", "Groceries")]
        [DataRow("locked", "0")]
        [TestMethod]
        public void GetsValueByName(string name, string expected)
        {
            // Arrange
            var row = GetDefaultRow();

            // Act
            var actual = row[name];

            // Assert
            Assert.AreEqual(expected, actual.TrimStart());
        }

        public void ThrowsWhenNoColumnFound()
        {
            // Arrange
            var row = GetDefaultRow();

            // Act - Assert
            Assert.ThrowsException<ParseException>(() => row["missing"]);
            Assert.ThrowsException<ParseException>(() => row["missing"] = "");
        }

        [DataRow("amount", "99.99")]
        [DataRow("subcategory", "Personal")]
        [DataRow("locked", "1")]
        public void SetsValueByName(string name, string value)
        {
            // Arrange
            var row = GetDefaultRow();

            // Act
            row[name] = value;

            // Assert
            row.Columns.Exists(column => column.ColumnValue.TrimStart() == value);
        }
    }
}
