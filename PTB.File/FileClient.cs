using Newtonsoft.Json;
using PTB.File.Categories;
using PTB.File.Budget;
using PTB.File.Ledger;
using PTB.File.TitleRegex;

namespace PTB.File
{
    public class FileClient
    {
        public LedgerRepository Ledger;
        public TitleRegexRepository Regex;
        public BudgetRepository Budget;
        public CategoriesRepository Categories;

        public void Instantiate(string home = @"C:\Users\abilson\OneDrive - SPR Consulting\Archive\2019\BudgetProject\PTB_Home\")
        {
            var settings = ReadSettingsFile(home);
            var schema = ReadSchemaFile(home);
            Ledger = new LedgerRepository(settings, schema);
            Regex = new TitleRegexRepository(settings, schema);
            Budget = new BudgetRepository(settings, schema);
            Categories = new CategoriesRepository(settings, schema);
        }

        private PTBSettings ReadSettingsFile(string home)
        {
            string settingsPath = System.IO.Path.Combine(home, "settings.json");
            PTBSettings settings = JsonConvert.DeserializeObject<PTBSettings>(System.IO.File.ReadAllText(settingsPath));
            return settings;
        }

        private PTBSchema ReadSchemaFile(string home)
        {
            string schemaPath = System.IO.Path.Combine(home, "schema.json");
            PTBSchema schema = JsonConvert.DeserializeObject<PTBSchema>(System.IO.File.ReadAllText(schemaPath));
            return schema;
        }
    }
}