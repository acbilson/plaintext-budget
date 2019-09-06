using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Exceptions;
using PTB.Core.Logging;
using PTB.Core.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PTB.Core.FolderAccess.Tests
{
    [TestClass]
    public class BaseFileServiceTests : GlobalSetup
    {
        [DataRow(@"C:\categories_19-01-01.txt", "categories_\\d{2}-\\d{2}-\\d{2}", true)]
        [DataRow(@"C:\category_19-01-01.txt", "categories_\\d{2}-\\d{2}-\\d{2}", false)]
        [DataRow(@"C:\Ledgers\ledger_checking_19-01-01_19-01-31.txt", "ledger_.*_\\d{2}-\\d{2}-\\d{2}_\\d{2}-\\d{2}-\\d{2}", true)]
        [DataRow(@"C:\ledger_checking_19-01-01.txt", "ledger_.*_\\d{2}-\\d{2}-\\d{2}_\\d{2}-\\d{2}-\\d{2}", false)]
        [TestMethod]
        public void ReturnsMatchingMask(string path, string mask, bool expected)
        {
            // Arrange
            var fileManager = new BaseFolderService(Settings, Schema, MockLogger.Object);

            // Act
            bool actual = fileManager.IsMaskMatch(path, mask);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
