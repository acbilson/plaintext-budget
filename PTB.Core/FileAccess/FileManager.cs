using System.Linq;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using PTB.Core.PTBFileAccess;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace PTB.Core
{
    public interface IFileManager
    {
        List<PTBFile> GetLedgerFiles();
        List<PTBFile> GetCategoriesFiles();

        FileInfo GetTitleRegexFile();
        FileInfo GetDefaultLedgerFile();
        FileInfo GetDefaultCategoriesFile();
    }

    public class FileManager : IFileManager
    {
        public PTBSettings Settings;
        public PTBSchema Schema;

        public FileManager(string baseDirectory)
        {
            GetConfigurationFromPath(baseDirectory); 
        }

        public FileManager(PTBSettings settings, PTBSchema schema)
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

        private List<PTBFile> GetFiles(string folder, string fileName, string fileMask)
        {
            var files = Directory.GetFiles(Path.Combine(Settings.HomeDirectory, folder))
                .Where(path => IsMaskMatch(path, fileMask))
                .Select(path => new PTBFile {
                    IsDefault = Path.GetFileNameWithoutExtension(path) == fileName,
                    Info = new FileInfo(path)
                }).ToList();
            return files;
        }

        private FileInfo GetFile(string folder, string fileName)
        {
            string path = Path.Combine(Settings.HomeDirectory, folder, fileName + Settings.FileExtension);
            return new FileInfo(path);
        }

        public List<PTBFile> GetLedgerFiles()
        {
            return GetFiles(Schema.Ledger.Folder, Schema.Ledger.DefaultFileName, Schema.Ledger.FileMask);
        }

        public FileInfo GetDefaultLedgerFile()
        {
            var files = GetLedgerFiles();
            return files.First(f => f.IsDefault == true).Info;
        }

        public List<PTBFile> GetCategoriesFiles()
        {
            return GetFiles(Schema.Categories.Folder, Schema.Categories.DefaultFileName, Schema.Categories.FileMask);
        }
        public FileInfo GetDefaultCategoriesFile()
        {
            var files = GetCategoriesFiles();
            return files.First(f => f.IsDefault == true).Info;
        }


        public FileInfo GetTitleRegexFile()
        {
            return GetFile(Schema.TitleRegex.Folder, Schema.TitleRegex.DefaultFileName);
        }


        /*
        private static readonly Lazy<FileManager> fileManager = new Lazy<FileManager>(() => new FileManager());

        private readonly string[] DEFAULT_DIRECTORIES = new string[] { LedgerFileOld.DIRECTORY, CategoriesFile.DIRECTORY, TitleRegexFile.DIRECTORY };
        private const string SETTINGS_FILE = "settings.json";

        private List<CategoriesFile> CategoriesFiles = new List<CategoriesFile>();
        private List<LedgerFileOld> LedgerFiles = new List<LedgerFileOld>();
        private PTBSettings Settings;
        private TitleRegexFile TitleRegexFile;

        public static FileManager Instance { get { return fileManager.Value; } }

        public void Instantiate(string baseDir) {
            // TODO: add default information to guide user in the use of these files
            CreateMissingFolderStructure(baseDir);
            PTBSettings settings = LoadSettingsFile(baseDir);
            LoadCategoriesFiles(baseDir);
            LoadLedgerFiles(baseDir, settings);
            LoadTitleRegexFiles(baseDir, settings);
            Settings = settings;
        }

        private void CreateMissingFolderStructure(string baseDir)
        {
            foreach(string dir in DEFAULT_DIRECTORIES)
            {
                string dirPath = Path.Combine(baseDir, dir);

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
        }

        private PTBSettings LoadSettingsFile(string baseDir)
        {
            string settingsPath = Path.Combine(baseDir, SETTINGS_FILE);

            if (!File.Exists(settingsPath))
            {
                File.Create(settingsPath);
            }

            PTBSettings settings = JsonConvert.DeserializeObject<PTBSettings>(File.ReadAllText(settingsPath));
            return settings;
        }

        private void LoadCategoriesFiles(string baseDir)
        {
            string categoriesDir = Path.Combine(baseDir, CategoriesFile.DIRECTORY);
            string[] existingCategoryFiles = Directory.GetFiles(categoriesDir);

            if (existingCategoryFiles.Length == 0)
            {
                string newCatFileName = CategoriesFile.GetNewFileName();
                string newCatFilePath = Path.Combine(categoriesDir, newCatFileName);
                File.Create(newCatFilePath);
            } else {
                foreach (string catFile in existingCategoryFiles)
                {
                    CategoriesFiles.Add(new CategoriesFile(catFile));
                }
            }
        }

        private void LoadLedgerFiles(string baseDir, PTBSettings settings)
        {
            string ledgerDir = Path.Combine(baseDir, LedgerFileOld.DIRECTORY);
            string[] existingLedgerFiles = Directory.GetFiles(ledgerDir);

            if (existingLedgerFiles.Length == 0)
            {
                string newLedgerFileName = LedgerFileOld.GetNewFileName(settings.DefaultLedgerName);
                string newLedgerFilePath = Path.Combine(ledgerDir, newLedgerFileName);
                File.Create(newLedgerFilePath);
            } else {
                foreach (string ledgerFile in existingLedgerFiles)
                {
                    LedgerFiles.Add(new LedgerFileOld(ledgerFile));
                }
            }
        }
        private void LoadTitleRegexFiles(string baseDir, PTBSettings settings)
        {
            string titleRegexDir = Path.Combine(baseDir, TitleRegexFile.DIRECTORY);
            string titleRegexPath = Path.Combine(titleRegexDir, settings.DefaultTitleRegexName + Constant.FILE_EXTENSION);

            if (!File.Exists(titleRegexPath))
            {
                string newTitleRegexFileName = TitleRegexFile.GetNewFileName();
                string newTitleRegexFilePath = Path.Combine(titleRegexDir, newTitleRegexFileName);
                File.Create(newTitleRegexFilePath);
            }

            TitleRegexFile = new TitleRegexFile(titleRegexPath);
        }

        public LedgerFileOld GetDefaultLedgerFile()
        {
            return LedgerFiles.FirstOrDefault((l) => l.LedgerName == Settings.DefaultLedgerName);
        }

        public TitleRegexFile GetDefaultTitleRegexFile()
        {
            return this.TitleRegexFile;
        }
    */
    }
}