using PTB.File.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.File.Categories
{
    public class CategoriesSchema
    {
        public string Delimiter { get; set; }

        public int Size { get; set; }

        public CategoriesColumns Columns { get; set; }
    }

    public class CategoriesColumns
    {
        public SchemaColumn Category { get; set; }
        public SchemaColumn Subcategory { get; set; }
    }
}
