using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.FileTypes
{
    public class TitleRegexFile
    {
        public static readonly string DIRECTORY = "Regex";
        public string FullName;
        public string FileName;

        public TitleRegexFile(string path)
        {
            FullName = path;
            FileName = System.IO.Path.GetFileName(FullName);
        }
        public static string GetNewFileName()
        {
            return $"title-regex{Constant.FILE_EXTENSION}";
        }
    }
}
