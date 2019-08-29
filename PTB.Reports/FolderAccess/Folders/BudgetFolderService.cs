using PTB.Core;
using PTB.Core.FolderAccess;
using PTB.Core.Logging;
using PTB.Reports.Budget;

namespace PTB.Reports.FolderAccess
{
    public class BudgetFolderService : BaseFolderService
    {
        public BudgetFolderService(PTBSettings settings, BudgetSchema schema, IPTBLogger logger) : base(settings, schema, logger)
        {
        }

        public PTBFolder<CategoriesFile> GetFolder()
        {
            return base.GetFolder<CategoriesFile>();
        }
    }
}