using PTB.File.Base;

namespace PTB.File.Categories
{
    public class CategoriesParser : BaseParser
    {
        private CategoriesSchema _schema;

        public CategoriesParser(CategoriesSchema schema)
        {
            _schema = schema;
        }

        public Categories ParseLine(string line)
        {
            int delimiterLength = _schema.Delimiter.Length;

            string category = CalculateByteIndex(delimiterLength, line, _schema.Columns.Category);
            string subcategory = CalculateByteIndex(delimiterLength, line, _schema.Columns.Subcategory);

            return new Categories(category, subcategory);
        }
    }
}