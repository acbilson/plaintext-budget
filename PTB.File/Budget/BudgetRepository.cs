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
    
        public void CreateBudget()
        {
            //List<Categories.Categories> categories = ReadAllCategories();

            // TODO: Group by major category
            // TODO: Print major category followed by subcategory
        }
    }
}