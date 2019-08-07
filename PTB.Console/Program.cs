using System;
using System.IO;
using PTB.Parser.Parsers;
using PTB.Parser.Objects;

namespace PlaintextBudget
{
    public class Program
    {
        static void Main(string[] args)
        {
            var parser = new PNCParser();

            using (var reader = new StreamReader(@"C:\Users\abilson\OneDrive - SPR Consulting\Working\Bench\Source\Resource\datafile.csv"))
            {
                string line = null;

                while ((line = reader.ReadLine()) != null) {

                    PNCTransaction transaction = parser.Parse(line);
                    Console.WriteLine(line);
                }
            }

            Console.ReadKey();
        }
    }
}
