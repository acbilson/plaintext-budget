using System;
using System.Collections.Generic;
using System.IO;
using PTB.Core;
using PTB.Core.Parsers;
using PTB.Core.FileTypes;

namespace PlaintextBudget
{
    public class Program
    {
        static void Main(string[] args)
        {
            string loadFilePath = @"C:\Users\abilson\OneDrive - SPR Consulting\Working\Bench\Source\Resource\datafile.csv";

            string home = Environment.GetEnvironmentVariable("ONEDRIVECOMMERCIAL");
            string baseDir = Path.Combine(home, @"Working\Bench\PTB_Home");
            var fileManager = FileManager.Instance;
            fileManager.Instantiate(baseDir);

            var parser = new PNCParser();
            LedgerFile defaultLedgerFile = fileManager.GetDefaultLedgerFile();

            using (var writer = new StreamWriter(defaultLedgerFile.FullName))
            {
                using (var reader = new StreamReader(loadFilePath))
                {
                    string line = null;

                    while ((line = reader.ReadLine()) != null)
                    {
                        Transaction transaction = parser.Parse(line);
                        writer.WriteLine(transaction);
                    }
                }
            }
        }
    }
}
