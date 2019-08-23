using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using Newtonsoft.Json;
using PTB.Files.Ledger;

namespace PTB.Core.Tests
{
    [TestClass]
    public class GlobalSetup
    {
        public LedgerSchema LedgerSchema;
        public FolderSchema Schema;
        public PTBSettings Settings;

        [TestInitialize]
        public void Initialize()
        {
            Schema = GetDefaultSchema();
            LedgerSchema = GetLedgerSchema();
            Settings = GetDefaultSettings();
        }

        public PTBSettings GetDefaultSettings()
        {
            return new PTBSettings
            {
                FileDelimiter = '_',
                FileExtension = ".txt",
                HomeDirectory = @"C:\Users\abilson\SourceCode\PlaintextBudget\TestOutput\netcoreapp2.1"

            };
        }

        public LedgerSchema GetLedgerSchema()
        {
            return new LedgerSchema();
        }

        public FolderSchema GetDefaultSchema()
        {
            var text = System.IO.File.ReadAllText("./schema.json");
            FolderSchema schema = JsonConvert.DeserializeObject<FolderSchema>(text);
            return schema;
        }
    }
}