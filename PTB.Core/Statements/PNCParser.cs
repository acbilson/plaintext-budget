using System.Linq;
using PTB.Core.Base;
using PTB.Core.Ledger;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace PTB.Core.Statements
{
    public class PNCParser : IStatementParser
    {
        private const char DELIMITER = ',';
        private FolderSchema _schema;

        public PNCParser(FolderSchema schema)
        {
            _schema = schema;
        }

        private ColumnSchema GetColumnByName(string name) => _schema.Columns.First(column => column.ColumnName.Equals(name, StringComparison.OrdinalIgnoreCase));

        public StatementParseResponse ParseLine(string line)
        {
            var response = StatementParseResponse.Default;

            if (IsSummaryLine(line))
            {
                response.Success = false;
                response.Message = "Skip summary line";
                return response;
            }

            string[] lines = line.Split(DELIMITER);

            string date = ParseDate(lines[0]);
            string amount = ParseAmount(lines[1]);
            string title = ParseTitle(lines[2], lines[3]);
            char type = ParseType(lines[5]);
            string subcategory = new String(' ', GetColumnByName("subcategory").Size);
            string subject = new String(' ', GetColumnByName("subject").Size);
            char locked = '0';

            char delimiter = ' ';
            var builder = new StringBuilder();
            builder.Append(date);
            builder.Append(delimiter);
            builder.Append(type);
            builder.Append(delimiter);
            builder.Append(amount);
            builder.Append(delimiter);
            builder.Append(subcategory);
            builder.Append(delimiter);
            builder.Append(subject);
            builder.Append(delimiter);
            builder.Append(title);
            builder.Append(delimiter);
            builder.Append(locked);

            response.Result = builder.ToString();
            return response;
        }

        // example summary line: "00000000004604718986,2019/06/18,2019/07/16,7320.66,7763.23"
        private bool IsSummaryLine(string line)
        {
            bool match = Regex.IsMatch(line, @"\d*\,\d{4}\/\d{2}\/\d{2}\,.*");
            return match;
        }

        private string PrependSpaces(string value, int max) => new String(' ', max - value.Length) + value;

        private string AddTrailingZeros(string value)
        {
            int missingCents = value.LastIndexOf('.') + 2 - value.Length;
            if (missingCents > 0)
            {
                value = new String('0', missingCents) + value;
            }
            return value;
        }

        private string ParseNoiseChars(string value)
        {
            Regex pattern = new Regex($"[{Constant.NOISE_CHARS}]");
            return pattern.Replace(value.Trim().ToLower(), string.Empty);
        }

        private string ParseDate(string value)
        {
            DateTime result;
            if (DateTime.TryParse(value, out result) == false)
            {
                // should skip this transaction
            }

            return result.ToString("yyyy-MM-dd");
        }

        private string ParseAmount(string value)
        {
            string amount = value;

            double result;
            if (double.TryParse(value, out result) == false)
            {
                // should skip this transaction
            }

            amount = AddTrailingZeros(amount);
            return PrependSpaces(amount, GetColumnByName("amount").Size);
        }

        private string ParseTitle(string value, string value2)
        {
            string title = value + value2;
            if (string.IsNullOrWhiteSpace(title))
            {
                // should skip this transaction
            }

            title = ParseNoiseChars(title);

            // crops title if it's too long
            // TODO: improve cropping logic
            int titleSize = GetColumnByName("title").Size;
            if (title.Length > titleSize)
            {
                title = title.Substring(0, titleSize);
            }

            return PrependSpaces(title, titleSize);
        }

        private char ParseType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // should skip this transaction
            }

            string type = value.Trim().Replace("'", string.Empty).Replace("\"", string.Empty);
            if (type == "DEBIT")
            {
                return 'D';
            }
            else
            {
                return 'C';
            }
        }
    }
}