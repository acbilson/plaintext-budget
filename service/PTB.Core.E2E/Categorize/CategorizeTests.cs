﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using System.Collections.Generic;

namespace PTB.E2E.Categorize
{
    [TestClass]
    [TestCategory("integration")]
    public class CategorizeTests : GlobalSetup
    {
        [TestMethod]
        public void CategorizesCorrectLedgerEntries()
        {
            // Arrange

            // Act
            WhenALedgerIsCategorized();
            List<PTBRow> ledgerEntries = WithAllLedgerEntries();

            // Assert
            ShouldHaveCategorizedAtLeastOneLedger(ledgerEntries);
            ShouldNotCategorizeLockedLedger(ledgerEntries);
        }
    }
}