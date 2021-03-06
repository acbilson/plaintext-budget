﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        public FolderSchema GetDefaultSchema()
        {
            var text = System.IO.File.ReadAllText(System.IO.Path.Combine(Settings.HomeDirectory, "schema.json"));
            FolderSchema schema = JsonConvert.DeserializeObject<FolderSchema>(text);
            return schema;
        }

        public void WithAMockLogger()
        {
            MockLogger = new Mock<IPTBLogger>();
        }
    }
}