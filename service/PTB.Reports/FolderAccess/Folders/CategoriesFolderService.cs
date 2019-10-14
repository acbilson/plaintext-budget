using PTB.Core;
using PTB.Core.FolderAccess;
using PTB.Core.Logging;
using PTB.Reports.Categories;

namespace PTB.Reports.FolderAccess
{
    public class CategoriesFolderService : BaseFolderService
    {
        public CategoriesFolderService(PTBSettings settings, CategoriesSchema schema, IPTBLogger logger) : base(settings, schema, logger)
        {
        }

        public PTBFolder<CategoriesFile> GetFolder()
        {
            return base.GetFolder<CategoriesFile>();
        }
    }
}