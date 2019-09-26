using PTB.Core.Logging;
using PTB.Core.Reports;
using System.Linq;
using System.Text;

namespace PTB.Core.Base
{
    public class BaseReportParser
    {
        protected FolderSchema _schema;
        protected IPTBLogger _logger;
        protected FileValidation _validator;

        public BaseReportParser(FolderSchema schema, IPTBLogger logger)
        {
            _schema = schema;
            _logger = logger;
            _validator = new FileValidation(_logger);
        }

        protected string CalculateByteIndex(int delimiterLength, string line, ColumnSchema column)
        {
            int start = column.Offset + (delimiterLength * (column.Index - 1));
            return line.Substring(start, column.Size);
        }

        public virtual RowToStringResponse ParseRow(PTBRow row)
        {
            var response = RowToStringResponse.Default;

            var builder = new StringBuilder();
            var columnValues = row.Columns
                .OrderBy(column => column.Index)
                .Select(column => ((ReportColumn)column).ColumnValue);
            builder.AppendJoin(_schema.Delimiter, columnValues);

            response.Line = builder.ToString();
            return response;
        }

        public virtual StringToRowResponse ParseLine(string line, int index)
        {
            var response = StringToRowResponse.Default;

            var validationResponse = _validator
                .LineEndsWithNewLine(line)
                .LineSizeMatchesSchema(line, _schema.LineSize)
                .Response;

            if (!validationResponse.Success)
            {
                response.Success = validationResponse.Success;
                response.Message = validationResponse.Message;
                return response;
            }

            foreach (ColumnSchema columnSchema in _schema.Columns)
            {
                var column = new ReportColumn(columnSchema);
                column.ColumnValue = CalculateByteIndex(_schema.Delimiter.Length, line, column);
                response.Row.Columns.Add(column);
            }

            return response;
        }
    }
}