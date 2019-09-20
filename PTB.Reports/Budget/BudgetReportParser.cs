using PTB.Core;
using PTB.Core.Base;
using PTB.Core.Logging;
using PTB.Core.Reports;
using System.Linq;
using System.Text.RegularExpressions;

namespace PTB.Reports.Budget
{
    public class BudgetReportParser : BaseReportParser
    {
        protected new BudgetSchema _schema;

        public BudgetReportParser(BudgetSchema schema, IPTBLogger logger) : base(schema, logger)
        {
            _schema = schema;
        }

        public override StringToRowResponse ParseLine(string line, int index)
        {
            var response = StringToRowResponse.Default;

            if (!LineEndsWithWindowsNewLine(line))
            {
                response.Success = false;
                response.Message = ParseMessages.LINE_NO_CR;
                return response;
            }

            if (!LineSizeMatchesSchema(line, _schema.LineSize))
            {
                response.Success = false;
                response.Message = ParseMessages.LINE_LENGTH_MISMATCH_SCHEMA;
                return response;
            }

            if (IsEmptyLine(line))
            {
                response.Success = true;
                response.Message = ParseMessages.EMPTY_LINE;
                return response;
            }

            if (IsSectionHeader(line))
            {
                var headerColumn = _schema.Columns.First(c => c.ColumnName == _schema.SectionHeader);
                var column = new ReportColumn(headerColumn);
                column.IsHeaderColumn = true;

                var value = CalculateByteIndex(_schema.Delimiter.Length, line, column);

                // strips report separator
                column.ColumnValue = value.TrimStart(_schema.CategorySeparator);
                response.Row.Columns.Add(column);
            }
            else
            {
                var nonHeaderColumns = _schema.Columns.Where(c => c.ColumnName != _schema.SectionHeader);

                foreach (ColumnSchema columnSchema in nonHeaderColumns)
                {
                    var column = new ReportColumn(columnSchema);
                    column.ColumnValue = CalculateByteIndex(_schema.Delimiter.Length, line, column);
                    response.Row.Columns.Add(column);
                }
            }

            response.Row.Index = index;

            return response;
        }

        public bool IsEmptyLine(string line) => Regex.IsMatch(line, "^\\s+$");

        public bool IsSectionHeader(string line) => line.Contains(_schema.CategorySeparator);
    }
}