using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.FileTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.Tests.FileTypes
{
    [TestClass]
    public class LedgerTests
    {
        [TestMethod]
        public void CorrectFileName()
        {
            // Arrange
            string baseDir = @"C:\Users\myuser\Documents";
            string name = "checking";
            string startDate = "19-01-01";
            string endDate = "19-12-31";

            // Act
            var ledger = new Ledger(baseDir, name, startDate, endDate);

            // Assert
            Assert.AreEqual("ledger_checking_19-01-01_19-12-31.txt", ledger.FileName);
        }

        [TestMethod]
        public void CorrectFullName()
        {
            // Arrange
            string baseDir = @"C:\Users\myuser\Documents";
            string name = "checking";
            string startDate = "19-01-01";
            string endDate = "19-12-31";

            // Act
            var ledger = new Ledger(baseDir, name, startDate, endDate);

            // Assert
            Assert.AreEqual(@"C:\Users\myuser\Documents\Ledgers\ledger_checking_19-01-01_19-12-31.txt", ledger.FullName);
        }
    }
}
