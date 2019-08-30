using PTB.Core.Base;
using PTB.Core.Logging;
using PTB.Core.Reports;
using PTB.Reports.FolderAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PTB.Reports.Budget
{
    public class BudgetService : BaseReportService
    {
        public BudgetService(IPTBLogger logger, BudgetFileParser parser, BudgetSchema schema) : base(logger, parser, schema)
        {
            _logger.SetContext(nameof(BudgetService));
        }

        public string GetCategoryString(string category)
        {
            var schema = (BudgetSchema)_schema;
            string separator = new String(schema.CategorySeparator, _schema["amount"].Size);
            return string.Concat(separator, _schema.Delimiter, category);
        }

        public string GetEmptyAmount()
        {
            string appendedZeros = "0.00 ";
            string emptyAmount = new String(' ', _schema["amount"].Size - appendedZeros.Length);
            return emptyAmount + appendedZeros;
        }

        public string GetSubcategoryString(string subcategory) => string.Concat(GetEmptyAmount(), _schema.Delimiter, subcategory);

        public void Create(BudgetFile file, List<PTBRow> categories)
        {
            // consider adding to CategoriesService. Needs tests
            var groupedCategories = categories.GroupBy(
                group => group["category"].TrimStart(),
                group => group["subcategory"].TrimStart(),
                (trimmedKey, group) => new
                {
                    Category = trimmedKey,
                    Subcategories = group.OrderBy(innerGroup => innerGroup.TrimStart())
                })
                .OrderBy(group => group.Category);

            using (var writer = new StreamWriter(file.FullPath, append: false))
            {
                foreach (var group in groupedCategories)
                {
                    writer.WriteLine(GetCategoryString(group.Category));

                    foreach (var subcategory in group.Subcategories)
                    {
                        writer.WriteLine(GetSubcategoryString(subcategory));
                    }

                    // adds a space between categories
                    writer.WriteLine(string.Empty);
                }
            }
        }
    }
}