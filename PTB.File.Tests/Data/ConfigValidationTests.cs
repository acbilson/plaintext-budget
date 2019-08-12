using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.File.Tests.Data
{
    [TestClass]
    public class ConfigValidationTests
    {
        [TestMethod]
        public void LoadsCleanSettings()
        {
            // Arrange / Act
            string settingsPath = @"./Data/Clean/settings.json";
            PTBSettings settings = JsonConvert.DeserializeObject<PTBSettings>(System.IO.File.ReadAllText(settingsPath));

            // Assert
            Assert.IsNotNull(settings.HomeDirectory);
            Assert.IsNotNull(settings.FileExtension);
            Assert.IsNotNull(settings.FileDelimiter);
            Assert.IsNotNull(settings.DefaultLedgerName);
            Assert.IsNotNull(settings.DefaultTitleRegexName);
        }

        [TestMethod]
        public void LoadsCleanSchema()
        {
            // Arrange / Act
            string settingsPath = @"./Data/Clean/schema.json";
            PTBSchema schema = JsonConvert.DeserializeObject<PTBSchema>(System.IO.File.ReadAllText(settingsPath));

            // Assert
            Assert.IsNotNull(schema.Ledger);
            Assert.IsNotNull(schema.Ledger.Delimiter);
            Assert.IsNotNull(schema.Ledger.Size);

            Assert.IsNotNull(schema.Ledger.Columns);
            Assert.IsNotNull(schema.Ledger.Columns.Amount);
            Assert.IsNotNull(schema.Ledger.Columns.Amount.Index);
            Assert.IsNotNull(schema.Ledger.Columns.Amount.Offset);
            Assert.IsNotNull(schema.Ledger.Columns.Amount.Size);
            Assert.IsNotNull(schema.Ledger.Columns.Date);
            Assert.IsNotNull(schema.Ledger.Columns.Date.Index);
            Assert.IsNotNull(schema.Ledger.Columns.Date.Offset);
            Assert.IsNotNull(schema.Ledger.Columns.Date.Size);
            Assert.IsNotNull(schema.Ledger.Columns.Location);
            Assert.IsNotNull(schema.Ledger.Columns.Location.Index);
            Assert.IsNotNull(schema.Ledger.Columns.Location.Offset);
            Assert.IsNotNull(schema.Ledger.Columns.Location.Size);
            Assert.IsNotNull(schema.Ledger.Columns.Locked);
            Assert.IsNotNull(schema.Ledger.Columns.Locked.Index);
            Assert.IsNotNull(schema.Ledger.Columns.Locked.Offset);
            Assert.IsNotNull(schema.Ledger.Columns.Locked.Size);
            Assert.IsNotNull(schema.Ledger.Columns.Subcategory);
            Assert.IsNotNull(schema.Ledger.Columns.Subcategory.Index);
            Assert.IsNotNull(schema.Ledger.Columns.Subcategory.Offset);
            Assert.IsNotNull(schema.Ledger.Columns.Subcategory.Size);
            Assert.IsNotNull(schema.Ledger.Columns.Title);
            Assert.IsNotNull(schema.Ledger.Columns.Title.Index);
            Assert.IsNotNull(schema.Ledger.Columns.Title.Offset);
            Assert.IsNotNull(schema.Ledger.Columns.Title.Size);
            Assert.IsNotNull(schema.Ledger.Columns.Type);
            Assert.IsNotNull(schema.Ledger.Columns.Type.Index);
            Assert.IsNotNull(schema.Ledger.Columns.Type.Offset);
            Assert.IsNotNull(schema.Ledger.Columns.Type.Size);

            Assert.IsNotNull(schema.Ledger.Files);
            Assert.IsTrue(schema.Ledger.Files.Length > 0, "Should have at least one file");
            Assert.IsNotNull(schema.Ledger.Files[0].IsDefault);
            Assert.IsNotNull(schema.Ledger.Files[0].Name);

            Assert.IsNotNull(schema.TitleRegex);
        }

    }
}
