using PTB.Core;
using PTB.Core.Logging;
using PTB.Core.Statements;
using PTB.Core.TitleRegex;
using System;
using System.IO;
using System.Text;

namespace PTB.Console
{
    public class Program
    {
        private static PTBClient InitiateClient()
        {
            string home = Environment.GetEnvironmentVariable("ONEDRIVECOMMERCIAL");
            string baseDir = Path.Combine(home, @"Working\Bench\PTB_Home");
            var client = new PTBClient();
            var fileManager = new FileManager(baseDir);
            client.Instantiate(fileManager, new PTBFileLogger(LoggingLevel.Debug, baseDir));
            return client;
        }

        private static void Main(string[] args)
        {
            bool exit = false;

            do
            {
                System.Console.WriteLine("Please enter a value, or '?' for help:");
                string input = System.Console.ReadLine();

                if (input == "?")
                {
                    var builder = new StringBuilder();
                    builder.Append("These are your options:");
                    builder.Append(System.Environment.NewLine);
                    builder.Append($" - Import Statement ('import')");
                    builder.Append(System.Environment.NewLine);
                    builder.Append($" - Categorize Default Ledger ('categorize')");
                    builder.Append(System.Environment.NewLine);
                    builder.Append($" - Generate Budget ('budget')");
                    builder.Append(System.Environment.NewLine);
                    builder.Append($" - Exit ('exit')");
                    builder.Append(System.Environment.NewLine);
                    System.Console.Write(builder.ToString());
                }

                if (input == "exit")
                {
                    exit = true;
                    continue;
                }

                if (input == "import")
                {
                    var client = InitiateClient();

                    var importFiles = client.FileManager.GetStatementFiles();

                    foreach (var importFile in importFiles)
                    {
                        var parser = new PNCParser();
                        client.Ledger.ImportToDefaultLedger(importFile.FullName, parser, append: true);
                    }
                }

                if (input == "categorize")
                {
                    var client = InitiateClient();

                    TitleRegexReadResponse response = client.Regex.ReadAllTitleRegex();
                    client.Ledger.CategorizeDefaultLedger(response.TitleRegices);

                    foreach (var skippedMessage in response.SkippedMessages)
                    {
                        System.Console.WriteLine(skippedMessage);
                    }
                }

                if (input == "budget")
                {
                    var client = InitiateClient();

                    var response = client.Categories.ReadAllDefaultCategories();
                    client.Budget.CreateBudget(response.Categories);
                }
            } while (exit == false);
        }
    }
}