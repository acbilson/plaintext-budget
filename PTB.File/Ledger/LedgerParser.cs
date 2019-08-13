using PTB.File.Base;
using System;
using System.Text;

namespace PTB.File.Ledger
{
    public class LedgerParser : BaseParser
    {
        private LedgerSchema _schema;

        public LedgerParser(LedgerSchema schema)
        {
            _schema = schema;
        }
        public Ledger ParseLine(string line, int index = 0)
        {
            int delimiterLength = _schema.Delimiter.Length;
            string date = CalculateByteIndex(delimiterLength, line, _schema.Columns.Date);
            string type = CalculateByteIndex(delimiterLength, line, _schema.Columns.Type);
            string amount = CalculateByteIndex(delimiterLength, line, _schema.Columns.Amount);
            string subcategory = CalculateByteIndex(delimiterLength, line, _schema.Columns.Subcategory);
            string title = CalculateByteIndex(delimiterLength, line, _schema.Columns.Title);
            string location = CalculateByteIndex(delimiterLength, line, _schema.Columns.Location);
            string locked = CalculateByteIndex(delimiterLength, line, _schema.Columns.Locked);

            return new Ledger(index, date, amount, title, location, Convert.ToChar(type), Convert.ToChar(locked), subcategory);
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
            builder.Append(ledger.Title);
            builder.Append(_schema.Delimiter);
            builder.Append(ledger.Location);
            builder.Append(_schema.Delimiter);
            builder.Append(ledger.Locked);
            return builder.ToString();
        }
    }
}