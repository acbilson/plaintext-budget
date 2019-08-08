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
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var parser = new PNCParser();
            var ledger = new Ledger(baseDir, "checking", "19-01-01", "19-12-31");

            using (var writer = new StreamWriter(@"C:\Users\abilson\OneDrive - SPR Consulting\Working\Bench\Source\Resource\ledger.csv"))
            {
                using (var reader = new StreamReader(@"C:\Users\abilson\OneDrive - SPR Consulting\Working\Bench\Source\Resource\datafile.csv"))
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
