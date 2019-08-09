using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.Parsers
{
    public class TransactionParser
    {
        public Transaction ParseLine(string line)
        {
            if (line.Length != Constant.TRANSACTION_SIZE)
            {
                // should skip this transaction
            }

            string date = line.Substring(TransactionColumnIndex.DATE[0], TransactionColumnIndex.DATE[1]);
            string amount = line.Substring(TransactionColumnIndex.AMOUNT[0], TransactionColumnIndex.AMOUNT[1]);
            string title = line.Substring(TransactionColumnIndex.TITLE[0], TransactionColumnIndex.TITLE[1]);
            string location = line.Substring(TransactionColumnIndex.LOCATION[0], TransactionColumnIndex.LOCATION[1]);
            string type = line.Substring(TransactionColumnIndex.TYPE[0], TransactionColumnIndex.TYPE[1]);
            string locked = line.Substring(TransactionColumnIndex.LOCKED[0], TransactionColumnIndex.LOCKED[1]);
            string subcategory = line.Substring(TransactionColumnIndex.SUBCATEGORY[0], TransactionColumnIndex.SUBCATEGORY[1]);

            return new Transaction(date, amount, title, location, Convert.ToChar(type), Convert.ToChar(locked), subcategory);
        }
    }
}
