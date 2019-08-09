namespace PTB.File.TitleRegex
{
    public struct TitleRegex
    {
        public char Priority;
        public string Regex;
        public string Subcategory;

        public TitleRegex(char priority, string regex, string subcategory)
        {
            Priority = priority;
            Regex = regex;
            Subcategory = subcategory;
        }
    }
}