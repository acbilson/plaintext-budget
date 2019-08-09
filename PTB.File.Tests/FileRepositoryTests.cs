using System;
using System.Collections.Generic;
using System.Text;
using PTB.File;
using PTB.Core;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace PTB.File.Tests
{
    [TestClass]
    public class FileRepositoryTests
    {
        [TestMethod]
        public void ConstructsFromCleanSettings()
        {
            // Arrange
            string text = System.IO.File.ReadAllText(@"Data\clean-settings.json");
            PTBSettings settings = JsonConvert.DeserializeObject<PTBSettings>(text);

            // Act
            //var repo = new BaseFileRepository(settings, FileType.Ledger);
        }
    }
}
