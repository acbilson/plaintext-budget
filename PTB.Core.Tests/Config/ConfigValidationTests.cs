using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace PTB.Core.Config.Tests
{
    [TestClass]
    public class ConfigValidationTests
    {
        [TestMethod]
        public void LoadsCleanSettings()
        {
            // Arrange / Act
            string settingsPath = @"./Clean/settings.json";
            PTBSettings settings = JsonConvert.DeserializeObject<PTBSettings>(System.IO.File.ReadAllText(settingsPath));

            // Assert
            Assert.IsNotNull(settings.HomeDirectory);
            Assert.IsNotNull(settings.FileExtension);
            Assert.IsNotNull(settings.FileDelimiter);
        }

        [TestMethod]
        public void LoadsCleanSchema()
        {
            // Arrange / Act
            string settingsPath = @"./schema.json";
            PTBSchema schema = JsonConvert.DeserializeObject<PTBSchema>(System.IO.File.ReadAllText(settingsPath));

            // Assert
            Assert.IsNotNull(schema.Ledger);
            Assert.IsNotNull(schema.Ledger.Delimiter);
            Assert.IsNotNull(schema.Ledger.LineSize);
            Assert.IsNotNull(schema.Ledger.DefaultFileName);
            Assert.IsNotNull(schema.Ledger.Folder);

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

            Assert.IsNotNull(schema.TitleRegex);
            Assert.IsNotNull(schema.TitleRegex.Delimiter);
            Assert.IsNotNull(schema.TitleRegex.LineSize);
            Assert.IsNotNull(schema.TitleRegex.DefaultFileName);
            Assert.IsNotNull(schema.TitleRegex.Folder);

            Assert.IsNotNull(schema.TitleRegex.Columns);
            Assert.IsNotNull(schema.TitleRegex.Columns.Priority);
            Assert.IsNotNull(schema.TitleRegex.Columns.Priority.Index);
            Assert.IsNotNull(schema.TitleRegex.Columns.Priority.Offset);
            Assert.IsNotNull(schema.TitleRegex.Columns.Priority.Size);
            Assert.IsNotNull(schema.TitleRegex.Columns.Subcategory);
            Assert.IsNotNull(schema.TitleRegex.Columns.Subcategory.Index);
            Assert.IsNotNull(schema.TitleRegex.Columns.Subcategory.Offset);
            Assert.IsNotNull(schema.TitleRegex.Columns.Subcategory.Size);
            Assert.IsNotNull(schema.TitleRegex.Columns.Regex);
            Assert.IsNotNull(schema.TitleRegex.Columns.Regex.Index);
            Assert.IsNotNull(schema.TitleRegex.Columns.Regex.Offset);
            Assert.IsNotNull(schema.TitleRegex.Columns.Regex.Size);
            Assert.IsNotNull(schema.TitleRegex.Columns.Subject);
            Assert.IsNotNull(schema.TitleRegex.Columns.Subject.Index);
            Assert.IsNotNull(schema.TitleRegex.Columns.Subject.Offset);
            Assert.IsNotNull(schema.TitleRegex.Columns.Subject.Size);

            Assert.IsNotNull(schema.Categories);
            Assert.IsNotNull(schema.Categories.Delimiter);
            Assert.IsNotNull(schema.Categories.LineSize);
            Assert.IsNotNull(schema.Categories.DefaultFileName);
            Assert.IsNotNull(schema.Categories.Folder);

            Assert.IsNotNull(schema.Categories.Columns);
            Assert.IsNotNull(schema.Categories.Columns.Category);
            Assert.IsNotNull(schema.Categories.Columns.Category.Index);
            Assert.IsNotNull(schema.Categories.Columns.Category.Offset);
            Assert.IsNotNull(schema.Categories.Columns.Category.Size);
            Assert.IsNotNull(schema.Categories.Columns);
            Assert.IsNotNull(schema.Categories.Columns.Subcategory);
            Assert.IsNotNull(schema.Categories.Columns.Subcategory.Index);
            Assert.IsNotNull(schema.Categories.Columns.Subcategory.Offset);
            Assert.IsNotNull(schema.Categories.Columns.Subcategory.Size);

            Assert.IsNotNull(schema.Budget);
            Assert.IsNotNull(schema.Budget.CategorySeparator);
            Assert.IsNotNull(schema.Budget.LineSize);
        }
    }
}