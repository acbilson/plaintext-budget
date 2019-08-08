using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.FileTypes
{
    public class LedgerFile
    {
        public static readonly string DIRECTORY = "Ledgers";
        public string FullName;
        public string FileName;
        public string LedgerName;
        public string LedgerStartDate;

        public LedgerFile(string path)
        {
            FullName = path;
            FileName = System.IO.Path.GetFileName(FullName);
            string[] fileNameParts = FileName.Split(Constant.FILE_DELIMITER);
            LedgerName = fileNameParts[1];
            LedgerStartDate = fileNameParts[2].Replace(Constant.FILE_EXTENSION, string.Empty);
        }

        public static string GetNewFileName(string name)
        {
            string startDate = DateTime.Now.ToString("yy-MM-dd");
            string endDate = new DateTime(DateTime.Now.Year, 12, 31).ToString("yy-MM-dd");
            return $"ledger{Constant.FILE_DELIMITER}{name.Trim()}{Constant.FILE_DELIMITER}{startDate}{Constant.FILE_DELIMITER}{endDate}{Constant.FILE_EXTENSION}";
        }
    }
}
