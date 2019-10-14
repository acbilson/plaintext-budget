using PTB.Core;
using PTB.Core.FolderAccess;
using PTB.Core.Logging;
using PTB.Files.TitleRegex;

namespace PTB.Files.FolderAccess
{
    public class TitleRegexFolderService : BaseFolderService
    {
        public TitleRegexFolderService(PTBSettings settings, TitleRegexSchema schema, IPTBLogger logger) : base(settings, schema, logger)
        {
        }

        public PTBFolder<TitleRegexFile> GetFolder()
        {
            return base.GetFolder<TitleRegexFile>();
        }
    }
}