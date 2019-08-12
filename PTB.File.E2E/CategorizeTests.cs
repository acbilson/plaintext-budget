using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.File.E2E
{
    [TestClass]
    public class CategorizeTests : GlobalSetup
    {
        [TestInitialize]
        public void Initialize()
        {
            string folder = "Categorize";
            GetDefaultSchema(folder);
            GetDefaultSettings(folder);
        }

        [TestMethod]
        public void CategorizesCorrectLedgerEntries()
        {
            // Arrange
            WithAFileClient();

            // Act
            IEnumerable<TitleRegex.TitleRegex> titleRegices = Client.Regex.ReadAllTitleRegex();
            Client.Ledger.CategorizeDefaultLedger(titleRegices);

            // Assert

        }
    }
}
