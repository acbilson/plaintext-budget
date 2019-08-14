namespace PTB.File.Base
{
    public class BaseParser
    {
        public bool LineEndsWithWindowsNewLine(string line) => line.IndexOf(System.Environment.NewLine) == (line.Length - System.Environment.NewLine.Length);

        public bool LineSizeMatchesSchema(string line, int schemaSize) => line.Length == (schemaSize + System.Environment.NewLine.Length);

        public string CalculateByteIndex(int delimiterLength, string line, SchemaColumn column)
        {
            int start = column.Offset + (delimiterLength * (column.Index - 1));
            return line.Substring(start, column.Size);
        }
    }
}