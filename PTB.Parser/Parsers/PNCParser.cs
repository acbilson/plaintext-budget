using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PTB.Core.Parsers
{
    public class PNCParser : IStatementParser
    {
        private const char DELIMITER = ',';

        public string ParseLine(string line)
        {
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
            return PrependSpaces(amount, TransactionColumnSize.AMOUNT);
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
            if (title.Length > TransactionColumnSize.TITLE)
            {
                title = title.Substring(0, TransactionColumnSize.TITLE);
            }

            return PrependSpaces(title, TransactionColumnSize.TITLE);
        }

        private string ParseLocation(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // should skip this transaction
            }

            string location = ParseNoiseChars(value);
            return PrependSpaces(location, TransactionColumnSize.LOCATION);
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
            } else
            {
                return 'C';
            }
        }

    }
}