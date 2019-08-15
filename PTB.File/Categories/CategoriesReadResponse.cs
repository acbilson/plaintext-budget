using System.Collections.Generic;

namespace PTB.File.Categories
{
    public class CategoriesReadResponse
    {
        public List<Categories> Categories;

        public List<string> SkippedMessages;

        public static CategoriesReadResponse Default => new CategoriesReadResponse { Categories = new List<Categories>(), SkippedMessages = new List<string>() };
    }
}