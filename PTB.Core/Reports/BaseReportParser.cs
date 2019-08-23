using System.Linq;
using System.Text;

namespace PTB.Core.Base
{
    public class BaseReportParser
    {
        private FolderSchema _schema;

        public BaseReportParser(FolderSchema schema)
        {
            _schema = schema;
        }

        protected bool LineEndsWithWindowsNewLine(string line) => line.IndexOf(System.Environment.NewLine) == (line.Length - System.Environment.NewLine.Length);

        protected bool LineSizeMatchesSchema(string line, int schemaSize) => line.Length == (schemaSize + System.Environment.NewLine.Length);

        protected string CalculateByteIndex(int delimiterLength, string line, ColumnSchema column)
        {
            int start = column.Offset + (delimiterLength * (column.Index - 1));
            return line.Substring(start, column.Size);
        }

        public RowToStringResponse ParseRow(PTBRow row)
        {
            var response = RowToStringResponse.Default;

            var builder = new StringBuilder();
            var columnValues = row.Columns.Select(column => column.ColumnValue);
            builder.AppendJoin(_schema.Delimiter, columnValues);

            response.Line = builder.ToString();
            return response;
        }

        public StringToRowResponse ParseLine(string line, int index)
        {
            var response = StringToRowResponse.Default;

            if (!LineEndsWithWindowsNewLine(line))
            {
                response.Success = false;
                response.Message = "Line does not end with carriage return, which may indicate data corruption";
                return response;
            }

            if (!LineSizeMatchesSchema(line, _schema.LineSize))
            {
                response.Success = false;
                response.Message = "Line length does not match schema, which may indicate data corruption.";
                return response;
            }

            foreach (ColumnSchema columnSchema in _schema.Columns)
            {
                var column = new PTBColumn(columnSchema);
                column.ColumnValue = CalculateByteIndex(_schema.Delimiter.Length, line, column);
                response.Row.Columns.Add(column);
            }

            return response;
        }
    }
}