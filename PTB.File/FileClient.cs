using Newtonsoft.Json;
using PTB.File.Ledger;
using PTB.File.TitleRegex;

namespace PTB.File
{
    public class FileClient
    {
        public LedgerRepository Ledger;
        public TitleRegexRepository Regex;

        public void Instantiate()
        {
            var settings = ReadSettingsFile();
            var schema = ReadSchemaFile();
            Ledger = new LedgerRepository(settings, schema);
            Regex = new TitleRegexRepository(settings, schema);
        }

        private PTBSettings ReadSettingsFile()
        {
            string settingsPath = @"C:\Users\abilson\OneDrive - SPR Consulting\Archive\2019\BudgetProject\PTB_Home\settings.json";
            PTBSettings settings = JsonConvert.DeserializeObject<PTBSettings>(System.IO.File.ReadAllText(settingsPath));
            return settings;
        }

        private PTBSchema ReadSchemaFile()
        {
            string schemaPath = @"C:\Users\abilson\OneDrive - SPR Consulting\Archive\2019\BudgetProject\PTB_Home\schema.json";
            PTBSchema schema = JsonConvert.DeserializeObject<PTBSchema>(System.IO.File.ReadAllText(schemaPath));
            return schema;
        }
    }
}