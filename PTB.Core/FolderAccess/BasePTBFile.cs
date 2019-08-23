namespace PTB.Core.FolderAccess
{
    public class BasePTBFile
    {
        public string DirectoryName { get; private set; }
        public string FileName { get; private set; }
        public string FullPath { get; private set; }
        public int LineCount { get; private set; }

        protected char _delimiter;

        public BasePTBFile() { }

        public BasePTBFile(char fileDelimiter, int lineSize, System.IO.FileInfo file)
        {
            _delimiter = fileDelimiter;
            LineCount = System.Convert.ToInt32(file.Length / lineSize);
            FullPath = file.FullName;
            FileName = file.Name;
            DirectoryName = file.DirectoryName;
        }
    }
}