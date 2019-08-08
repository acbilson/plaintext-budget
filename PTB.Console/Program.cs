using System;
using System.Collections.Generic;
using System.IO;
using PTB.Core;
using PTB.Core.Parsers;
using PTB.Core.FileTypes;

namespace PTB.Console
{
    enum ConsoleActions
    {
        Import,
        Categorize
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var action = ConsoleActions.Categorize;

            string home = Environment.GetEnvironmentVariable("ONEDRIVECOMMERCIAL");
            string baseDir = Path.Combine(home, @"Working\Bench\PTB_Home");
            var fileManager = FileManager.Instance;
            fileManager.Instantiate(baseDir);
            LedgerFile defaultLedgerFile = fileManager.GetDefaultLedgerFile();

            switch (action)
            {
                case ConsoleActions.Import:

                    string loadFilePath = @"C:\Users\abilson\OneDrive - SPR Consulting\Working\Bench\Source\Resource\datafile.csv";

                    var pncParser = new PNCParser();

                    using (var writer = new StreamWriter(defaultLedgerFile.FullName))
                    {
                        using (var reader = new StreamReader(loadFilePath))
                        {
                            string line = null;

                            while ((line = reader.ReadLine()) != null)
                            {
                                Transaction transaction = pncParser.Parse(line);
                                writer.WriteLine(transaction);
                            }
                        }
                    }

                    break;

                case ConsoleActions.Categorize:

                    List<TitleRegex> keys = new List<TitleRegex>();
                    var titleParser = new TitleRegexParser();
                    TitleRegexFile titleRegexFile = fileManager.GetDefaultTitleRegexFile();

                    using (var reader = new StreamReader(titleRegexFile.FullName))
                    {
                        string line = null;

                        while ((line = reader.ReadLine()) != null)
                        {
                            TitleRegex titleSubcategoriesKey = titleParser.Parse(line);
                            System.Console.WriteLine(titleSubcategoriesKey.ToString());
                            keys.Add(titleSubcategoriesKey);
                        }
                    }

                    break;

                default:

                    break;
            }
        }
    }
}
