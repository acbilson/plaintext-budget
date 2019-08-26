using PTB.Core;
using PTB.Core.Logging;

namespace PTB.Files.FolderAccess
{
    public class FileFolderService
    {
        private FileSchema _schema;
        private PTBSettings _settings;
        private IPTBLogger _logger;

        public FileFolderService(PTBSettings settings, FileSchema schema, IPTBLogger logger)
        {
            _schema = schema;
            _settings = settings;
            _logger = logger;
        }

        public FileFolders GetFileFolders()
        {
            var fileDirectory = new FileFolders();
            var ledgerFolderMgr = new LedgerFolderService(_settings, _schema.Ledger, _logger);
            var categoriesFolderMgr = new CategoriesFolderService(_settings, _schema.Categories, _logger);
            var titleRegexFolderMgr = new TitleRegexFolderService(_settings, _schema.TitleRegex, _logger);
            fileDirectory.LedgerFolder = ledgerFolderMgr.GetFolder();
            fileDirectory.CategoriesFolder = categoriesFolderMgr.GetFolder();
            fileDirectory.TitleRegexFolder = titleRegexFolderMgr.GetFolder();
            return fileDirectory;
        }
    }
}