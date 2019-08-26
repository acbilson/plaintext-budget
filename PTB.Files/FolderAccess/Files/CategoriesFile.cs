using PTB.Core.FolderAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Files.FolderAccess
{
    public class CategoriesFile : BasePTBFile
    {
        public DateTime StartDate { get; private set; }

        public CategoriesFile() { }

        public CategoriesFile(char fileDelimiter, int lineSize, System.IO.FileInfo file): base(fileDelimiter, lineSize, file)
        {
            string[] fileParts = GetFileNameParts(file.Name);
            StartDate = ParseDate(fileParts[1]);
        }
    }
}
