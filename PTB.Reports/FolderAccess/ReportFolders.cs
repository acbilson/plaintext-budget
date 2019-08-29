using PTB.Core.FolderAccess;

namespace PTB.Reports.FolderAccess
{
    public class ReportFolders
    {
        public PTBFolder<CategoriesFile> CategoriesFolder { get; set; }
        public PTBFolder<CategoriesFile> BudgetFolder { get; set; }
    }
}