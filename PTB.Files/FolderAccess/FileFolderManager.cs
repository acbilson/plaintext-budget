using PTB.Core;
using PTB.Core.Logging;
using PTB.Files.Ledger;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Files.FolderAccess
{
    public class FileFolderManager
    {
        private FileSchema _schema;
        private PTBSettings _settings;
        private IPTBLogger _logger;

        public FileFolderManager(PTBSettings settings, FileSchema schema, IPTBLogger logger)
        {
            _schema = schema;
            _settings = settings;
            _logger = logger;
        }

        public FileFolders GetFileFolders()
        {
            var directory = new FileFolders();
            var ledgerFolderMgr = new LedgerFolderManager(_settings, _schema.Ledger, _logger);
            directory.LedgerFolder = ledgerFolderMgr.GetFolder();
            return directory;
        }

    }
}
