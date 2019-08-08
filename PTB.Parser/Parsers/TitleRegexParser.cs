using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.Parsers
{
    public class TitleRegexParser
    {
        public TitleRegex Parse(string line)
        {
            if (line.Length != Constant.CATEGORIES_SIZE)
            {
                // should skip line
            }

            string priority = line.Substring(TitleRegexColumnIndex.PRIORITY[0], TitleRegexColumnIndex.PRIORITY[1]);
            string subcategory = line.Substring(TitleRegexColumnIndex.SUBCATEGORY[0], TitleRegexColumnIndex.SUBCATEGORY[1]);
            string regex = line.Substring(TitleRegexColumnIndex.REGEX[0], TitleRegexColumnIndex.REGEX[1]);

            return new TitleRegex(Convert.ToChar(priority), subcategory, regex);
        }
    }
}
