using System;
using PTB.Parser.Objects;

namespace PTB.Parser.Parsers
{
    public class PNCParser
    {
        const char DELIMITER = ',';

        public PNCTransaction Parse(string line)
        {
            string[] lines = line.Split(DELIMITER);

            return new PNCTransaction();
        }

        public string ParseDate(string value)
        {
            DateTime result;
            if (DateTime.TryParse(value, out result) == false)
            {
                // should skip this transaction
            }

            return result.ToString("yy-MM-dd");
        }
    }
} 