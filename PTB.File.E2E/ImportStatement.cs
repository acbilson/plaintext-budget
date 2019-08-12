using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.File.E2E
{
    [TestClass]
    public class ImportStatement : GlobalSetup
    {
        [TestMethod]
        public void ImportsCleanStatement()
        {
            // Arrange
            var client = WithAFileClient();
            var parser = WithAPNCParser();

            // Act
            client.Ledger.ImportToDefaultLedger(@".\Import\Clean\datafile.csv", parser);

            // Assert

        }
    }
}
