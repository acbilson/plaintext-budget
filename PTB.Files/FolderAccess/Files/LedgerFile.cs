using PTB.Core.FolderAccess;
using System;

namespace PTB.Files.FolderAccess
{
    public class LedgerFile : BasePTBFile
    {
        public string LedgerName { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public LedgerFile()
        {
        }

        public LedgerFile(char fileDelimiter, int lineSize, System.IO.FileInfo file) : base(fileDelimiter, lineSize, file)
        {
            string[] fileParts = GetFileNameParts(file.Name);
            LedgerName = fileParts[1];
            StartDate = ParseDate(fileParts[2]);
            StartDate = ParseDate(fileParts[3]);
        }
    }
}