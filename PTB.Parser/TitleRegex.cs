using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core
{
    public struct TitleRegex
    {
        public char Priority;
        public string Subcategory;
        public string Regex;

        public TitleRegex(char priority, string subcategory, string regex)
        {
            Priority = priority;
            Subcategory = subcategory;
            Regex = regex;
        }

        public override string ToString()
        {
            char delimiter = ' ';
            var builder = new StringBuilder();
            builder.Append(Priority);
            builder.Append(delimiter);
            builder.Append(Subcategory);
            builder.Append(delimiter);
            builder.Append(Regex);
            return builder.ToString();
        }

    }
}
