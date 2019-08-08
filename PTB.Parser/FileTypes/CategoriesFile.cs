using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.FileTypes
{
    public class CategoriesFile
    {
        public static readonly string DIRECTORY = "Categories";
        public string FullName;

        public CategoriesFile(string path)
        {
            FullName = path;
        }

        public static string GetNewFileName()
        {
            string startDate = DateTime.Now.ToString("yy-MM-dd");
            return $"categories{Constant.FILE_DELIMITER}{startDate}{Constant.FILE_EXTENSION}";
        }

    }
}
