using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core
{
    public struct Transaction
    {
        public string Date, Amount, Title, Location, Subcategory;
        public char Type, Locked;

        public Transaction(string date, string amount, string title, string location, char type)
        {
            Date = date;
            Amount = amount;
            Title = title;
            Location = location;
            Type = type;
            Subcategory = new String(' ', ColumnSize.SUBCATEGORY);
            Locked = '0';
        }
        public Transaction(string date, string amount, string title, string location, char type, char locked, string subcategory)
        {
            Date = date;
            Amount = amount;
            Title = title;
            Location = location;
            Type = type;
            Subcategory = subcategory;
            Locked = locked;
        }
 
        public override string ToString()
        {
            char delimiter = ' ';
            var builder = new StringBuilder();
            builder.Append(Date);
            builder.Append(delimiter);
            builder.Append(Type);
            builder.Append(delimiter);
            builder.Append(Amount);
            builder.Append(delimiter);
            builder.Append(Subcategory);
            builder.Append(delimiter);
            builder.Append(Title);
            builder.Append(delimiter);
            builder.Append(Location);
            builder.Append(delimiter);
            builder.Append(Locked);
            return builder.ToString();
        }
    }
}
