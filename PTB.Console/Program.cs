using PTB.File;
using PTB.File.Statements;
using System;
using System.Collections.Generic;
using System.IO;

namespace PTB.Console
{
    internal enum ConsoleActions
    {
        Import,
        Categorize
    }

    public class Program
    {
        private static void Main(string[] args)
        {
            var action = ConsoleActions.Categorize;

            string home = Environment.GetEnvironmentVariable("ONEDRIVECOMMERCIAL");
            string baseDir = Path.Combine(home, @"Working\Bench\PTB_Home");
            //var fileManager = FileManager.Instance;
            //fileManager.Instantiate(baseDir);
            //LedgerFileOld defaultLedgerFile = fileManager.GetDefaultLedgerFile();
            var client = new FileClient();
            client.Instantiate();

            switch (action)
            {
                case ConsoleActions.Import:

                    string loadFilePath = @"C:\Users\abilson\OneDrive - SPR Consulting\Working\Bench\Source\Resource\datafile.csv";
                    var parser = new PNCParser();
                    client.Ledger.ImportToDefaultLedger(loadFilePath, parser);

                    break;

                case ConsoleActions.Categorize:

                    IEnumerable<PTB.File.TitleRegex.TitleRegex> titleRegexes = client.Regex.ReadAllTitleRegex();
                    client.Ledger.CategorizeDefaultLedger(titleRegexes);

                    break;

                default:

                    break;
            }
        }
    }
}