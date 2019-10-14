using PTB.Core;
using PTB.Core.FolderAccess;
using PTB.Core.Logging;
using PTB.Files.Ledger;

namespace PTB.Files.FolderAccess
{
    public class LedgerFolderService : BaseFolderService
    {
        public LedgerFolderService(PTBSettings settings, LedgerSchema schema, IPTBLogger logger) : base(settings, schema, logger)
        {
        }

        public PTBFolder<LedgerFile> GetFolder()
        {
            return base.GetFolder<LedgerFile>();
        }
    }
}