using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PTB.Core.Base
{

    public class BaseFileManager
    {
        public PTBSettings Settings;
        public PTBSchema Schema;

        public BaseFileManager(string baseDirectory)
        {
            GetConfigurationFromPath(baseDirectory);
        }

        public BaseFileManager(PTBSettings settings, PTBSchema schema)
        {
            Settings = settings;
            Schema = schema;
        }

        private void GetConfigurationFromPath(string baseDirectory)
        {
            string settingsPath = System.IO.Path.Combine(baseDirectory, "settings.json");
            Settings = JsonConvert.DeserializeObject<PTBSettings>(System.IO.File.ReadAllText(settingsPath));

            string schemaPath = System.IO.Path.Combine(baseDirectory, "schema.json");
            Schema = JsonConvert.DeserializeObject<PTBSchema>(System.IO.File.ReadAllText(schemaPath));
        }

        private bool IsMaskMatch(string path, string fileMask) {
            string fileName = Path.GetFileNameWithoutExtension(path);
            return Regex.IsMatch(fileName, fileMask);

        }

        private List<BasePTBFile> GetFiles(string folder, string fileName, string fileMask)
        {
            var files = Directory.GetFiles(Path.Combine(Settings.HomeDirectory, folder))
                .Where(path => IsMaskMatch(path, fileMask))
                .Select(path => new BasePTBFile(Settings.FileDelimiter) {
                    FullPath = new FileInfo(path).FullName
                }).ToList();
            return files;
        }

        private string GetFile(string folder, string fileName)
        {
            string path = Path.Combine(Settings.HomeDirectory, folder, fileName + Settings.FileExtension);
            return new FileInfo(path).FullName;
        }

        public List<string> GetStatementFilePaths()
        {
             List<string> filePaths = Directory.GetFiles(Path.Combine(Settings.HomeDirectory, "Import"), "*.csv")
                .Select(path => new FileInfo(path).FullName).ToList();
            return filePaths;
        }

        public List<BasePTBFile> GetLedgerFiles()
        {
            return GetFiles(Schema.Ledger.Folder, Schema.Ledger.DefaultFileName, Schema.Ledger.FileMask);
        }
        public List<BasePTBFile> GetCategoriesFiles()
        {
            return GetFiles(Schema.Categories.Folder, Schema.Categories.DefaultFileName, Schema.Categories.FileMask);
        }

        public string GetTitleRegexFilePath()
        {
            return GetFile(Schema.TitleRegex.Folder, Schema.TitleRegex.DefaultFileName);
        }
    }
}
