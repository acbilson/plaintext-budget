namespace PTB.Core.TitleRegex
{
    public struct TitleRegex
    {
        public char Priority;
        public string Regex;
        public string Subcategory;
        public string Subject;

        public TitleRegex(char priority, string subcategory, string regex, string subject)
        {
            Priority = priority;
            Regex = regex;
            Subcategory = subcategory;
            Subject = subject;
        }
    }
}