using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Moq;
using PTB.Core.Logging;
using PTB.Report;
using PTB.Core;

namespace PTB.Reports.Tests
{
    [TestClass]
    public class GlobalSetup
    {
        public ReportSchema Schema;
        public PTBSettings Settings;
        public Mock<IPTBLogger> MockLogger;

        [TestInitialize]
        public void Initialize()
        {
            Settings = GetDefaultSettings();
            Schema = GetDefaultSchema();
            WithAMockLogger();
        }

        public PTBSettings GetDefaultSettings()
        {
            return new PTBSettings
            {
                FileDelimiter = '_',
                FileExtension = ".txt",
                WindowsHomeDirectory = @"C:\Users\abilson\SourceCode\PlaintextBudget\TestOutput\netcoreapp2.1\Clean",
                UnixHomeDirectory = @"/mnt/c/Users/abilson/SourceCode/PlaintextBudget/TestOutput/netcoreapp2.1/Clean"
            };
        }

        public ReportSchema GetDefaultSchema()
        {
            var text = System.IO.File.ReadAllText(System.IO.Path.Combine(Settings.HomeDirectory, "schema.json"));
            ReportSchema schema = JsonConvert.DeserializeObject<ReportSchema>(text);
            return schema;
        }
        public void WithAMockLogger()
        {
            MockLogger = new Mock<IPTBLogger>();
        }

    }
}