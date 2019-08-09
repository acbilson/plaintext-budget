using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.File.Ledger
{
    public struct Ledger
    {
        public string Date, Amount, Title, Location, Subcategory;
        public char Type, Locked;

        public Ledger(string date, string amount, string title, string location, char type, char locked, string subcategory)
        {
            Date = date;
            Amount = amount;
            Title = title;
            Location = location;
            Type = type;
            Subcategory = subcategory;
            Locked = locked;
        }
    }
}
