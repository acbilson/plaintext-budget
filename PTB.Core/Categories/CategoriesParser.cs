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

            if (!LineEndsWithWindowsNewLine(line))
            {
                response.Success = false;
                response.Message = "Line does not end with carriage return, which may indicate data corruption";
                return response;
            }

            if (!LineSizeMatchesSchema(line, _schema.Size))
            {
                response.Success = false;
                response.Message = "Line length does not match schema, which may indicate data corruption.";
                return response;
            }

            int delimiterLength = _schema.Delimiter.Length;

            string category = CalculateByteIndex(delimiterLength, line, _schema.Columns.Category);
            string subcategory = CalculateByteIndex(delimiterLength, line, _schema.Columns.Subcategory);

            response.Result = new Categories(category, subcategory);
            return response;
        }
    }
}