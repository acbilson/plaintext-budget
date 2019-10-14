using PTB.Core;
using PTB.Core.Logging;
using PTB.Report;

namespace PTB.Reports.FolderAccess
{
    public class ReportFolderService
    {
        private ReportSchema _schema;
        private PTBSettings _settings;
        private IPTBLogger _logger;

        public ReportFolderService(PTBSettings settings, ReportSchema schema, IPTBLogger logger)
        {
            _schema = schema;
            _settings = settings;
            _logger = logger;
        }

        public ReportFolders GetFolders()
        {
            var reportDirectory = new ReportFolders();
            var categoriesFolderMgr = new CategoriesFolderService(_settings, _schema.Categories, _logger);
            var budgetFolderMgr = new BudgetFolderService(_settings, _schema.Budget, _logger);
            reportDirectory.CategoriesFolder = categoriesFolderMgr.GetFolder();
            reportDirectory.BudgetFolder = budgetFolderMgr.GetFolder();
            return reportDirectory;
        }
    }
}