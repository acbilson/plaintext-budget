using PTB.Core.FolderAccess;
using System;
using System.Globalization;

namespace PTB.Reports.FolderAccess
{
    public class BudgetFile : BasePTBFile
    {
        public BudgetFile() { }
        public BudgetFile(char fileDelimiter, int lineSize, System.IO.FileInfo file) : base(fileDelimiter, lineSize, file)
        {
            string[] fileParts = GetFileNameParts(file.Name);
            StartDate = base.ParseDate(fileParts[1]);
            EndDate = ParseEndDate(fileParts[1], fileParts[3]);
        }

        public DateTime ParseEndDate(string startDate, string endDay) => DateTime.ParseExact(startDate, "yy-MM-dd", CultureInfo.InvariantCulture).AddDays(int.Parse(endDay) - 1);
    }
}