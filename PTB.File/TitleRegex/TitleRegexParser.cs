using PTB.File.Base;
using System;

namespace PTB.File.TitleRegex
{
    public class TitleRegexParser : BaseParser
    {
        private TitleRegexSchema _schema;

        public TitleRegexParser(TitleRegexSchema schema)
        {
            _schema = schema;
        }

        public TitleRegex ParseLine(string line)
        {
            int delimiterLength = _schema.Delimiter.Length;
            string priority = CalculateByteIndex(delimiterLength, line, _schema.Columns.Priority);
            string subcategory = CalculateByteIndex(delimiterLength, line, _schema.Columns.Subcategory);
            string regex = CalculateByteIndex(delimiterLength, line, _schema.Columns.Regex);

            return new TitleRegex(Convert.ToChar(priority), subcategory, regex);
        }
    }
}