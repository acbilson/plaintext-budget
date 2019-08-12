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
            var client = new FileClient();
            client.Instantiate();

            switch (action)
            {
                case ConsoleActions.Import:

                    string importFolderPath = System.IO.Path.Combine(baseDir, "Import");
                    string[] importFilePaths = System.IO.Directory.GetFiles(importFolderPath);

                    foreach (var importFilePath in importFilePaths)
                    {
                        var parser = new PNCParser();
                        client.Ledger.ImportToDefaultLedger(importFilePath, parser, append: true);
                    }

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