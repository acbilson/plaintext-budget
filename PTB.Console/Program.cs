using Microsoft.Extensions.DependencyInjection;
using PTB.Core;
using PTB.Core.Base;
using PTB.Core.Logging;
using PTB.Core.Statements;
using PTB.Files.FolderAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using PTB.Files.Ledger;
using PTB.Core.Files;

namespace PTB.Console
{
    public class Program
    {
        private static void Main(string[] args)
        {

            string home = Environment.GetEnvironmentVariable("ONEDRIVECOMMERCIAL");
            string baseDir = Path.Combine(home, @"Archive\2019\BudgetProject\PTB_Home");

            var settingsText = File.ReadAllText(Path.Combine(baseDir, "settings.json"));
            var settings = JsonConvert.DeserializeObject<PTBSettings>(settingsText);

            var schemaText = File.ReadAllText(Path.Combine(baseDir, "schema.json"));
            var schema = JsonConvert.DeserializeObject<FileSchema>(schemaText);

            var logger = new PTBFileLogger(settings.LoggingLevel, baseDir);

            // Uses same DI framework as Web for consistency
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IPTBLogger>(logger)
                .AddSingleton<FileSchema>(schema)
                .AddSingleton<PTBSettings>(settings)
                .AddSingleton<LedgerSchema>(schema.Ledger)
                .AddSingleton<LedgerFileParser>()
                .AddScoped<FileFolderManager>()
                .AddScoped<LedgerService>()
            .BuildServiceProvider();

            var fileFolderManager = serviceProvider.GetService<FileFolderManager>();
            var ledgerRepository = serviceProvider.GetService<LedgerService>();

            var fileFolders = fileFolderManager.GetFileFolders();
            var defaultLedgerFile = fileFolders.LedgerFolder.GetDefaultFile();
            var ledgers = ledgerRepository.Read(defaultLedgerFile, 0, 10);
        }
        /*
        private static PTBClient InitiateClient()
        {
            string home = Environment.GetEnvironmentVariable("ONEDRIVECOMMERCIAL");
            string baseDir = Path.Combine(home, @"Working\Bench\PTB_Home");
            var fileManager = new BaseFileManager(baseDir);
            var logger = new PTBFileLogger(LoggingLevel.Debug, home);
            return new PTBClient(baseDir);
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

                    List<string> importFilePaths = client.FileManager.GetStatementFilePaths();

                    foreach (var importPath in importFilePaths)
                    {
                        var parser = new PNCParser();
                        client.Ledger.ImportToDefaultLedger(importPath, parser, append: true);
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
        */
    }
}
