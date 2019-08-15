using PTB.Core.Base;

namespace PTB.Core.Categories
{
    public class CategoriesParser : BaseParser
    {
        private CategoriesSchema _schema;

        public CategoriesParser(CategoriesSchema schema)
        {
            _schema = schema;
        }

        public StringToCategoriesResponse ParseLine(string line)
        {
            var response = StringToCategoriesResponse.Default;

            int delimiterLength = _schema.Delimiter.Length;

            string category = CalculateByteIndex(delimiterLength, line, _schema.Columns.Category);
            string subcategory = CalculateByteIndex(delimiterLength, line, _schema.Columns.Subcategory);

            response.Result = new Categories(category, subcategory);
            return response;
        }
    }
}