﻿using Newtonsoft.Json;
using PTB.Core.Categories;
using PTB.Core.Budget;
using PTB.Core.Ledger;
using PTB.Core.TitleRegex;
using PTB.Core.Logging;

namespace PTB.Core
{
    public class PTBClient
    {
        public FileManager FileManager;

        public LedgerRepository Ledger;
        public TitleRegexRepository Regex;
        public BudgetRepository Budget;
        public CategoriesRepository Categories;

        public void Instantiate(FileManager fileManager, IPTBLogger logger)
        {
            FileManager = fileManager;
            Ledger = new LedgerRepository(fileManager);
            Regex = new TitleRegexRepository(fileManager);
            Budget = new BudgetRepository(fileManager);
            Categories = new CategoriesRepository(fileManager, logger);
        }

        private PTBSchema ReadSchemaFile(string home)
        {
            string schemaPath = System.IO.Path.Combine(home, "schema.json");
            PTBSchema schema = JsonConvert.DeserializeObject<PTBSchema>(System.IO.File.ReadAllText(schemaPath));
            return schema;
        }
    }
}