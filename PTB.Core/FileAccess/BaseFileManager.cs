using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using PTB.Core.Base;
using PTB.Core.Files;

namespace PTB.Core.FileAccess
{

    public class BaseFileManager
    {
        public PTBSettings Settings;
        public FolderSchema Schema;

        public BaseFileManager(string baseDirectory)
        {
            GetConfigurationFromPath(baseDirectory);
        }

        public BaseFileManager(PTBSettings settings, FolderSchema schema)
        {
            Settings = settings;
            Schema = schema;
        }

        private void GetConfigurationFromPath(string baseDirectory)
        {
            string settingsPath = System.IO.Path.Combine(baseDirectory, "settings.json");
            Settings = JsonConvert.DeserializeObject<PTBSettings>(System.IO.File.ReadAllText(settingsPath));

            string schemaPath = System.IO.Path.Combine(baseDirectory, "schema.json");
            Schema = JsonConvert.DeserializeObject<FolderSchema>(System.IO.File.ReadAllText(schemaPath));
        }

        private bool IsMaskMatch(string path, string fileMask) {
            string fileName = Path.GetFileNameWithoutExtension(path);
            return Regex.IsMatch(fileName, fileMask);

        }

        private List<PTBFile> GetFiles(string folder, string fileName, string fileMask)
        {
            var files = Directory.GetFiles(Path.Combine(Settings.HomeDirectory, folder))
                .Where(path => IsMaskMatch(path, fileMask))
                .Select(path => new PTBFile(Settings.FileDelimiter) {
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
    }
}
