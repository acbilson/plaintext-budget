using PTB.Core;
using System;
using System.Linq;

namespace PTB.Core.Parsers
{
    public class PNCParser
    {
        private const char DELIMITER = ',';

        public Transaction Parse(string line)
        {
            string[] lines = line.Split(DELIMITER);

            string date = ParseDate(lines[0]);
            string amount = ParseAmount(lines[1]);
            string title = ParseTitle(lines[2], lines[3]);
            string location = ParseLocation(lines[4]);
            char type = ParseType(lines[5]);

            return new Transaction(date, amount, title, location, type);
        }

        public string PrependSpaces(string value, int maxLength)
        {
            return new String(' ', maxLength - value.Length) + value;
        }

        public string ParseDate(string value)
        {
            DateTime result;
            if (DateTime.TryParse(value, out result) == false)
            {
                // should skip this transaction
            }

            return result.ToString("yyyy-MM-dd");
        }

        public string ParseAmount(string value)
        {
            string amount = value;

            double result;
            if (double.TryParse(value, out result) == false)
            {
                // should skip this transaction
            }

            // adds a trailing zero
            int missingCents = amount.LastIndexOf('.') + 2 - amount.Length;
            if (missingCents > 0)
            {
                amount = new String('0', missingCents) + amount;
            }

            return PrependSpaces(amount, 12);
        }

        public string ParseTitle(string value, string value2)
        {
            int maxLength = 50;
            string title = value + value2;
            if (string.IsNullOrWhiteSpace(title))
            {
                // should skip this transaction
            }

            title = title.Trim().ToLower().Replace(" ", string.Empty).Replace("'", string.Empty).Replace("\"", string.Empty);

            // crops title if it's too long
            // TODO: improve cropping logic
            if (title.Length > maxLength)
            {
                title = title.Substring(0, maxLength);
            }

            return PrependSpaces(title, maxLength);
        }

        public string ParseLocation(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // should skip this transaction
            }

            string location = value.Trim().ToLower().Replace("'", string.Empty).Replace("\"", string.Empty);
            return PrependSpaces(location, 15);
        }

        public char ParseType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // should skip this transaction

            }

            string type = value.Trim().Replace("'", string.Empty);
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