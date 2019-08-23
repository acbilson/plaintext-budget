﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using Newtonsoft.Json;

namespace PTB.Core.Tests
{
    [TestClass]
    public class GlobalSetup
    {
        public FolderSchema Schema;
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
    }
}