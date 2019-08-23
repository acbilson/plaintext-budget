namespace PTB.Core.Files
{
    public class PTBFile
    {
        public string FullName { get; set; }
        public string FullPath { get; set; }
        public int LineCount { get; set; }

        protected int FirstDelimiterIndex => FullName.IndexOf(_delimiter);
        protected int SecondDelimiterIndex => FullName.IndexOf(_delimiter, FirstDelimiterIndex + 1);
        public string FileType => FullName.Substring(0, FirstDelimiterIndex);
        public string FileName => FullName.Substring(FirstDelimiterIndex, SecondDelimiterIndex);

        private char _delimiter;

        public PTBFile(char fileDelimiter)
        {
            _delimiter = fileDelimiter;
        }
    }
}