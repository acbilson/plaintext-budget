using PTB.Core.FolderAccess;

namespace PTB.Files.FolderAccess
{
    public class FileFolders
    {
        public PTBFolder<LedgerFile> LedgerFolder { get; set; }
        public PTBFolder<TitleRegexFile> TitleRegexFolder { get; set; }
        public PTBFolder<CategoriesFile> CategoriesFolder { get; set; }
    }
}