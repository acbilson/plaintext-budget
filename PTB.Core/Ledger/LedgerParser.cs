using PTB.Core.Base;
using System;
using System.Text;

namespace PTB.Core.Ledger
{
    public class LedgerParser : BaseParser
    {
        private LedgerSchema _schema;

        public LedgerParser(LedgerSchema schema)
        {
            _schema = schema;
        }

        public StringToLedgerResponse ParseLine(string line, int index = 0)
        {
            var response = StringToLedgerResponse.Default;

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

            int delimiterLength = _schema.Delimiter.Length;
            string date = CalculateByteIndex(delimiterLength, line, _schema.Columns.Date);
            string type = CalculateByteIndex(delimiterLength, line, _schema.Columns.Type);
            string amount = CalculateByteIndex(delimiterLength, line, _schema.Columns.Amount);
            string subcategory = CalculateByteIndex(delimiterLength, line, _schema.Columns.Subcategory);
            string subject = CalculateByteIndex(delimiterLength, line, _schema.Columns.Subject);
            string title = CalculateByteIndex(delimiterLength, line, _schema.Columns.Title);
            string locked = CalculateByteIndex(delimiterLength, line, _schema.Columns.Locked);

            response.Result = new Ledger(index, date, amount, subject, title, Convert.ToChar(type), Convert.ToChar(locked), subcategory);
            return response;
        }

        public string ParseLedger(Ledger ledger)
        {
            var builder = new StringBuilder();
            builder.Append(ledger.Date);
            builder.Append(_schema.Delimiter);
            builder.Append(ledger.Type);
            builder.Append(_schema.Delimiter);
            builder.Append(ledger.Amount);
            builder.Append(_schema.Delimiter);
            builder.Append(ledger.Subcategory);
            builder.Append(_schema.Delimiter);
            builder.Append(ledger.Subject);
            builder.Append(_schema.Delimiter);
            builder.Append(ledger.Title);
            builder.Append(_schema.Delimiter);
            builder.Append(ledger.Locked);
            return builder.ToString();
        }
    }
}