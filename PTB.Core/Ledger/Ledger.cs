namespace PTB.Core.Ledger
{
    public struct Ledger
    {
        public int Index;
        public string Date, Amount, Subject, Title, Subcategory;
        public char Type, Locked;

        public Ledger(int index, string date, string amount, string subject, string title, char type, char locked, string subcategory)
        {
            Index = index;
            Date = date;
            Amount = amount;
            Subject = subject;
            Title = title;
            Type = type;
            Subcategory = subcategory;
            Locked = locked;
        }
    }
}