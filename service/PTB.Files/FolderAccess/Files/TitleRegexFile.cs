using PTB.Core.FolderAccess;

namespace PTB.Files.FolderAccess
{
    public class TitleRegexFile : BasePTBFile
    {
        public TitleRegexFile()
        {
        }

        public TitleRegexFile(char fileDelimiter, int lineSize, System.IO.FileInfo file) : base(fileDelimiter, lineSize, file)
        {
        }
    }
}