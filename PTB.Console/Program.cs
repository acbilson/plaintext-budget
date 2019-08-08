using System;
using System.Collections.Generic;
using System.IO;
using PTB.Core;
using PTB.Core.Parsers;

namespace PlaintextBudget
{
    public class Program
    {
        static void Main(string[] args)
        {
            var parser = new PNCParser();

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
