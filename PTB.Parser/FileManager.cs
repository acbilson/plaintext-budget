﻿using PTB.Core.FileTypes;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PTB.Core
{
    public sealed class FileManager
    {
        private static readonly Lazy<FileManager> fileManager = new Lazy<FileManager>(() => new FileManager());

        private readonly string[] DEFAULT_DIRECTORIES = new string[] { LedgerFile.DIRECTORY, CategoriesFile.DIRECTORY, TitleRegexFile.DIRECTORY };
        private const string SETTINGS_FILE = "settings.json";

        private List<CategoriesFile> CategoriesFiles = new List<CategoriesFile>();
        private List<LedgerFile> LedgerFiles = new List<LedgerFile>();
        private FileSettings Settings;
        private TitleRegexFile TitleRegexFile;

        public static FileManager Instance { get { return fileManager.Value; } }

        public void Instantiate(string baseDir) {

            // TODO: add default information to guide user in the use of these files
            CreateMissingFolderStructure(baseDir);
            FileSettings settings = LoadSettingsFile(baseDir);
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

        private FileSettings LoadSettingsFile(string baseDir)
        {
            string settingsPath = Path.Combine(baseDir, SETTINGS_FILE);

            if (!File.Exists(settingsPath))
            {
                File.Create(settingsPath);
            }

            FileSettings settings = JsonConvert.DeserializeObject<FileSettings>(File.ReadAllText(settingsPath));
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

        private void LoadLedgerFiles(string baseDir, FileSettings settings)
        {
            string ledgerDir = Path.Combine(baseDir, LedgerFile.DIRECTORY);
            string[] existingLedgerFiles = Directory.GetFiles(ledgerDir);

            if (existingLedgerFiles.Length == 0)
            {
                string newLedgerFileName = LedgerFile.GetNewFileName(settings.DefaultLedgerName);
                string newLedgerFilePath = Path.Combine(ledgerDir, newLedgerFileName);
                File.Create(newLedgerFilePath);
            } else {
                foreach (string ledgerFile in existingLedgerFiles)
                {
                    LedgerFiles.Add(new LedgerFile(ledgerFile));
                }
            }
        }
        private void LoadTitleRegexFiles(string baseDir, FileSettings settings)
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

        public LedgerFile GetDefaultLedgerFile()
        {
            return LedgerFiles.FirstOrDefault((l) => l.LedgerName == Settings.DefaultLedgerName);
        }

        public TitleRegexFile GetDefaultTitleRegexFile()
        {
            return this.TitleRegexFile;
        }
    }
}
