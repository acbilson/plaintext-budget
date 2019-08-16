using PTB.Core.Base;
using System.Linq;

namespace PTB.Core.Categories
{
    public class CategoriesSchema : FolderSchema
    {
        public string FileMask { get; set; }
        public CategoriesColumns Columns { get; set; }
    }

    public class CategoriesColumns
    {
        public SchemaColumn Category { get; set; }
        public SchemaColumn Subcategory { get; set; }
    }
}