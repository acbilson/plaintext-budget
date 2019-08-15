using PTB.File.Categories;
using System.Collections.Generic;
using System.IO;

namespace PTB.File.Budget
{
    public class BudgetRepository : BaseFileRepository
    {
        private CategoriesParser _parser;

        public BudgetRepository(PTBSettings settings, PTBSchema schema) : base(settings, schema)
        {
            _parser = new CategoriesParser(schema.Categories);
        }

        public List<Categories.Categories> ReadAllCategories()
        {
            var categories = new List<Categories.Categories>();
            string categoriesPath = Path.Combine(_settings.HomeDirectory, @"Categories\categories.txt");
            string line;

            using (var reader = new StreamReader(categoriesPath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    Categories.Categories category = _parser.ParseLine(line);
                    categories.Add(category);
                }
            }

            return categories;
        }

        public void CreateBudget()
        {
            List<Categories.Categories> categories = ReadAllCategories();

            // TODO: Group by major category
            // TODO: Print major category followed by subcategory
        }
    }
}