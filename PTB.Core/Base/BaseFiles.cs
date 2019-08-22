using System.Linq;
using System.Collections.Generic;

namespace PTB.Core.Base
{
    public class PTBFolder
    {
        public string DefaultFileName { get; set; }
        public string Name { get; set; }
        public List<BasePTBFile> Files { get; set; }

        public BasePTBFile GetDefault() => Files.First(file => file.FileName == DefaultFileName);
    }

    public class BasePTBFile
    {
        public string FullName { get; set; }
        public string FullPath { get; set; }
        public int LineCount { get; set; }

        protected int FirstDelimiterIndex => FullName.IndexOf(_delimiter);
        protected int SecondDelimiterIndex => FullName.IndexOf(_delimiter, FirstDelimiterIndex + 1);
        public string FileType => FullName.Substring(0, FirstDelimiterIndex);
        public string FileName => FullName.Substring(FirstDelimiterIndex, SecondDelimiterIndex);

        private char _delimiter;
        public BasePTBFile(char fileDelimiter)
        {
            _delimiter = fileDelimiter;

        }
    }
}
