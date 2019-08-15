namespace PTB.Core.Ledger
{
    public struct Ledger
    {
        public int Index;
        public string Date, Amount, Title, Location, Subcategory;
        public char Type, Locked;

        public Ledger(int index, string date, string amount, string title, string location, char type, char locked, string subcategory)
        {
            Index = index;
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