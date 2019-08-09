using PTB.Core.FileTypes;
using PTB.Core.Parsers;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using PTB.Core;
using PTB.File.Ledger;

namespace PTB.File
{
    public class FileClient
    {
        public LedgerRepository Ledger; 

        public void Instantiate()
        {
            var settings = ReadSettingsFile();
            Ledger = new LedgerRepository(settings);

        }

        private PTBSettings ReadSettingsFile()
        {
            string settingsPath = @"C:\Users\abilson\OneDrive - SPR Consulting\Archive\2019\BudgetProject\PTB_Home\settings.json";
             PTBSettings settings = JsonConvert.DeserializeObject<PTBSettings>(System.IO.File.ReadAllText(settingsPath));
            return settings;
        }

    }
}
