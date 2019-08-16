using PTB.Core.Categories;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace PTB.Core.Budget
{
    public class BudgetRepository : BaseFileRepository
    {
        private string _folder = "Budget";
        private CategoriesParser _parser;

        public BudgetRepository(FileManager fileManager) : base(fileManager)
        {
            _parser = new CategoriesParser(_schema.Categories);
        }

        public string GetBudgetName()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            string startDate = new DateTime(year, month, 1).ToString("yy-MM-dd");
            string endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("yy-MM-dd");
            return $"budget{_settings.FileDelimiter}{startDate}{_settings.FileDelimiter}{endDate}";
        }

        public string GetCategoryString(string category)
        {
            string separator = new String(_schema.Budget.CategorySeparator, _schema.Ledger.Columns.Amount.Size);
            return string.Concat(separator, _schema.Categories.Delimiter, category);
        }

        public string GetEmptyAmount()  {
            string appendedZeros = "0.00 ";
            string emptyAmount = new String(' ', _schema.Ledger.Columns.Amount.Size - appendedZeros.Length);
            return emptyAmount + appendedZeros;
        }
        public string GetSubcategoryString(string subcategory) => string.Concat(GetEmptyAmount(), _schema.Categories.Delimiter, subcategory);
    
        public void CreateBudget(List<Categories.Categories> categories)
        {
            var path = base.GetDefaultPath(_folder, GetBudgetName());

            // creates the Budget directory if this is the first time creating a budget
            Directory.CreateDirectory(new FileInfo(path).Directory.FullName);
            /*var query = grades.GroupBy(student => student.Name)
                              .Select(group => 
                                    new { Name = group.Key,
                                          Students = group.OrderByDescending(x => x.Grade) })
                              .OrderBy(group => group.Students.First().Grade); */
            var groupedCategories = categories.GroupBy(
                group => group.Category.TrimStart(),
                group => group.Subcategory.TrimStart(),
                (trimmedKey, group) => new {
                    Category = trimmedKey,
                    Subcategories = group.OrderBy(innerGroup => innerGroup.TrimStart())
                })
                .OrderBy(group => group.Category);

            using (var writer = new StreamWriter(path, append: false))
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