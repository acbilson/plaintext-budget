using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.File.Categories
{
    public struct Categories
    {
        public string Category, Subcategory;

        public Categories(string category, string subcategory)
        {
            Category = category;
            Subcategory = subcategory;
        }
    }
}
