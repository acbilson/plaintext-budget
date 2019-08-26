using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using Newtonsoft.Json;
using PTB.Core.Logging;
using Moq;

namespace PTB.Core.Tests
{
    [TestClass]
    public class GlobalSetup
    {
        public FolderSchema Schema;
        public PTBSettings Settings;

        public Mock<IPTBLogger> MockLogger;


        [TestInitialize]
        public void Initialize()
        {
            Schema = GetDefaultSchema();
            Settings = GetDefaultSettings();
            WithAMockLogger();
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

        public FolderSchema GetDefaultSchema()
        {
            var text = System.IO.File.ReadAllText("./schema.json");
            FolderSchema schema = JsonConvert.DeserializeObject<FolderSchema>(text);
            return schema;
        }

        public void WithAMockLogger()
        {
            MockLogger = new Mock<IPTBLogger>();
        }
    }
}