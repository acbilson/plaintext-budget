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

        private string GetCategoryString(string category)
        {
            var schema = (BudgetSchema)_schema;
            string separator = new String(schema.CategorySeparator, _schema["amount"].Size);
            return string.Concat(separator, _schema.Delimiter, category);
        }

        private string GetEmptyAmount()
        {
            string appendedZeros = "0.00 ";
            string emptyAmount = new String(' ', _schema["amount"].Size - appendedZeros.Length);
            return emptyAmount + appendedZeros;
        }

        private string GetSubcategoryString(string subcategory) => string.Concat(GetEmptyAmount(), _schema.Delimiter, subcategory);

        private bool IsLastCategory(int index, int categoryCount) => index != categoryCount - 1;

        public void Create(BudgetFile file, List<PTBRow> categories)
        {
            // consider adding to CategoriesService. Needs tests
            var groupedCategories = categories.GroupBy(
                group => group["category"],
                group => group["subcategory"],
                (key, group) => new
                {
                    Category = key,
                    Subcategories = group.OrderBy(innerGroup => innerGroup)
                })
                .OrderBy(group => group.Category);

            using (var writer = new StreamWriter(file.FullPath, append: false))
            {
                for (int i = 0; i < groupedCategories.Count(); i++)
                {
                    writer.WriteLine(GetCategoryString(groupedCategories.ElementAt(i).Category));

                    foreach (var subcategory in groupedCategories.ElementAt(i).Subcategories)
                    {
                        writer.WriteLine(GetSubcategoryString(subcategory));
                    }

                    // adds an empty line between categories, except if it's the last line
                    if (IsLastCategory(i, groupedCategories.Count()))
                    {
                        writer.WriteLine(new string(' ', _schema.LineSize));
                    }
                }
            }
        }

        public BaseReadResponse Read(BudgetFile file, int index, int count)
        {
            var response = BaseReadResponse.Default;

            return response;
        }
    }
}