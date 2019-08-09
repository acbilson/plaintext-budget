namespace PTB.File.Base
{
    public class BaseParser
    {
        public string CalculateByteIndex(int delimiterLength, string line, SchemaColumn column)
        {
            int start = column.Offset + (delimiterLength * (column.Index - 1));
            return line.Substring(start, column.Size);
        }
    }
}