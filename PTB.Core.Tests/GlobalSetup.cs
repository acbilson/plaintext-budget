using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using PTB.Core.Ledger;
using PTB.Core.TitleRegex;
using Newtonsoft.Json;

namespace PTB.Core.Tests
{
    [TestClass]
    public class GlobalSetup
    {
        public PTBSchema Schema;
        public PTBSettings Settings;

        [TestInitialize]
        public void Initialize()
        {
            Schema = GetDefaultSchema();
            Settings = GetDefaultSettings();
        }

        public PTBSettings GetDefaultSettings()
        {
            return new PTBSettings
            {
                FileDelimiter = "_",
                FileExtension = ".txt",
                HomeDirectory = @"C:\Users\abilson\SourceCode\PlaintextBudget\TestOutput\netcoreapp2.1"

            };
        }

        public PTBSchema GetDefaultSchema()
        {
            var text = System.IO.File.ReadAllText("./schema.json");
            PTBSchema schema = JsonConvert.DeserializeObject<PTBSchema>(text);
            return schema;
        }
    }
}