using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Parser.Objects
{
    public struct PNCTransaction
    {
        public string Date, Amount, Title, Location;
        public char Type;

        public PNCTransaction(string date, string amount, string title, string location, char type)
        {
            Date = date;
            Amount = amount;
            Title = title;
            Location = location;
            Type = type;
        }
    }
}
