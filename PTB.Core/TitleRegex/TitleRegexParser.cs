using PTB.Core.Base;
using System;

namespace PTB.Core.TitleRegex
{
    public class TitleRegexParser : BaseParser
    {
        private TitleRegexSchema _schema;

        public TitleRegexParser(TitleRegexSchema schema)
        {
            _schema = schema;
        }

        public StringToTitleRegexResponse ParseLine(string line)
        {
            var response = StringToTitleRegexResponse.Default;

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
            string priority = CalculateByteIndex(delimiterLength, line, _schema.Columns.Priority);
            string subcategory = CalculateByteIndex(delimiterLength, line, _schema.Columns.Subcategory);
            string regex = CalculateByteIndex(delimiterLength, line, _schema.Columns.Regex);

            response.Result = new TitleRegex(Convert.ToChar(priority), subcategory, regex);
            return response;
        }
    }
}