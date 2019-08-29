using PTB.Core.FolderAccess;
using System;

namespace PTB.Reports.FolderAccess
{
    public class CategoriesFile : BasePTBFile
    {
        public CategoriesFile()
        {
        }

        public CategoriesFile(char fileDelimiter, int lineSize, System.IO.FileInfo file) : base(fileDelimiter, lineSize, file)
        {
            string[] fileParts = GetFileNameParts(file.Name);
            StartDate = ParseDate(fileParts[1]);
        }
    }
}