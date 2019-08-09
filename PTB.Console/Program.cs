using System;
using System.Collections.Generic;
using System.IO;
using PTB.Core;
using PTB.Core.Parsers;
using PTB.Core.FileTypes;
using System.Text;
using PTB.File;

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
            var action = ConsoleActions.Import;

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

                    List<TitleRegex> keys = new List<TitleRegex>();
                    var titleParser = new TitleRegexParser();
                    TitleRegexFile titleRegexFile = new TitleRegexFile("");//fileManager.GetDefaultTitleRegexFile();

                    using (var stream = System.IO.File.Open(titleRegexFile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                    {
                        byte[] buffer = new byte[65];
                        while (stream.Read(buffer, 0, Constant.CATEGORIES_SIZE + 2) > 0)
                        {
                            string line = Encoding.UTF8.GetString(buffer);
                            System.Console.Write(line);
                            TitleRegex result = titleParser.Parse(line);
                            string newLine = line.Replace("1", "2");
                            System.Console.Write(newLine);
                            byte[] newBytes = Encoding.UTF8.GetBytes(newLine);
                            stream.Write(newBytes, 0, 65);
                        }
                        /*
                                                while ((line = stream.ReadLine()) != null)
                                                {
                                                    TitleRegex titleRegex = titleParser.Parse(line);
                                                    keys.Add(titleRegex);

                                                    // Read transactions here
                                                    // Use regex to check each transaction against the list of regexes 
                                                    // Update the transaction file with the new categories (needs research)
                                                }
                                                */
                    }

                    break;

                default:

                    break;
            }
        }
    }
}
