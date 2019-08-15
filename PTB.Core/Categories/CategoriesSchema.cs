using PTB.Core.Base;
using System.Linq;

namespace PTB.Core.Categories
{
    public class CategoriesSchema
    {
        public string Delimiter { get; set; }

        public int Size { get; set; }

        public CategoriesColumns Columns { get; set; }

        public PTBFiles[] Files { get; set; }
        public string GetDefaultName()
        {
            return Files.First((l) => l.IsDefault == true).Name;
        }
    }

    public class CategoriesColumns
    {
        public SchemaColumn Category { get; set; }
        public SchemaColumn Subcategory { get; set; }
    }
}