using PTB.File.Ledger;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace PTB.File.Statements
{
    public class PNCParser : IStatementParser
    {
        private const char DELIMITER = ',';
        private LedgerSchema _schema;

        public string ParseLine(string line, LedgerSchema schema)
        {
            _schema = schema;

            string[] lines = line.Split(DELIMITER);

            string date = ParseDate(lines[0]);
            string amount = ParseAmount(lines[1]);
            string title = ParseTitle(lines[2], lines[3]);
            string location = ParseLocation(lines[4]);
            char type = ParseType(lines[5]);
            string subcategory = new String(' ', 20);
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
            builder.Append(title);
            builder.Append(delimiter);
            builder.Append(location);
            builder.Append(delimiter);
            builder.Append(locked);
            return builder.ToString();
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
            return PrependSpaces(amount, _schema.Columns.Amount.Size);
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
            if (title.Length > _schema.Columns.Title.Size)
            {
                title = title.Substring(0, _schema.Columns.Title.Size);
            }

            return PrependSpaces(title, _schema.Columns.Title.Size);
        }

        private string ParseLocation(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // should skip this transaction
            }

            string location = ParseNoiseChars(value);
            return PrependSpaces(location, _schema.Columns.Location.Size);
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