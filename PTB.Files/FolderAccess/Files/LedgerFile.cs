using PTB.Core.FolderAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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

        public LedgerFile(char fileDelimiter, int lineSize, System.IO.FileInfo file): base(fileDelimiter, lineSize, file)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            string[] fileParts = fileName.Split(fileDelimiter);
            LedgerName = fileParts[1];
            StartDate = DateTime.ParseExact(fileParts[2], "yy-MM-dd", CultureInfo.InvariantCulture);
            EndDate = DateTime.ParseExact(fileParts[3], "yy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}
