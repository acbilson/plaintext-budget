using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.File.Base
{
    public class BaseParser
    {
        public string CalculateByteIndex(int delimiterLength, string line, SchemaColumn column)
        {
            int delimiterOffset = delimiterLength;
            int start = column.Offset + (delimiterOffset * (column.Index - 1));
            int end = start + column.Size;

            return line.Substring(start, end);
        }
    }
}
