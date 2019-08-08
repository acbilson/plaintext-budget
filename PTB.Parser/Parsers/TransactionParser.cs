using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.Parsers
{
    public class TransactionParser : BaseParser
    {
        public override Transaction Parse(string line)
        {
            if (line.Length != Constant.TRANSACTION_SIZE)
            {
                // should skip this transaction
            }

            string date = line.Substring(ColumnIndex.DATE[0], ColumnIndex.DATE[1]);
            string amount = line.Substring(ColumnIndex.AMOUNT[0], ColumnIndex.AMOUNT[1]);
            string title = line.Substring(ColumnIndex.TITLE[0], ColumnIndex.TITLE[1]);
            string location = line.Substring(ColumnIndex.LOCATION[0], ColumnIndex.LOCATION[1]);
            string type = line.Substring(ColumnIndex.TYPE[0], ColumnIndex.TYPE[1]);
            string locked = line.Substring(ColumnIndex.LOCKED[0], ColumnIndex.LOCKED[1]);
            string subcategory = line.Substring(ColumnIndex.SUBCATEGORY[0], ColumnIndex.SUBCATEGORY[1]);


            return new Transaction(date, amount, title, location, Convert.ToChar(type), Convert.ToChar(locked), subcategory);
        }
    }
}
