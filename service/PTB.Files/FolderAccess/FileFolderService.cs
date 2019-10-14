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

        public FileFolders GetFolders()
        {
            var fileDirectory = new FileFolders();
            var ledgerFolderMgr = new LedgerFolderService(_settings, _schema.Ledger, _logger);
            var titleRegexFolderMgr = new TitleRegexFolderService(_settings, _schema.TitleRegex, _logger);
            fileDirectory.LedgerFolder = ledgerFolderMgr.GetFolder();
            fileDirectory.TitleRegexFolder = titleRegexFolderMgr.GetFolder();
            return fileDirectory;
        }

        public FileSchema GetFileSchema()
        {
            var schemaText = System.IO.File.ReadAllText(System.IO.Path.Combine(_settings.HomeDirectory, "schema.json"));
            var schema = Newtonsoft.Json.JsonConvert.DeserializeObject<FileSchema>(schemaText);
            return schema;
        }
    }
}