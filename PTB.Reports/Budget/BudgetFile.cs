using PTB.Core.FolderAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Reports.Budget
{
    public class BudgetFile : BasePTBFile
    {
        public DateTime StartDate { get; private set; }

        public BudgetFile(char fileDelimiter, int lineSize, System.IO.FileInfo file): base(fileDelimiter, lineSize, file)
        {
            string[] fileParts = file.Name.Split(fileDelimiter);
            StartDate = Convert.ToDateTime(fileParts[1]);
        }
    }
}
