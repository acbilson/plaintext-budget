using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.FileTypes
{
    public class Ledger
    {
        private const string DELIMITER = "_";
        private const string TYPE = "ledger";
        private const string DIRECTORY = "Ledgers";
        private const string EXTENSION = ".txt";

        private string BaseDir, Name, StartDate, EndDate;

        public string FileName => $"{TYPE}{DELIMITER}{Name}{DELIMITER}{StartDate}{DELIMITER}{EndDate}{EXTENSION}";

        public string FullName => System.IO.Path.Combine(BaseDir, DIRECTORY, FileName);

        public Ledger(string baseDir, string name, string startDate, string endDate)
        {
            BaseDir = baseDir;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
