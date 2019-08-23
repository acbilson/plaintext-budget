using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PTB.Core.Base;
using PTB.Core.Logging;
using System.Text.RegularExpressions;

namespace PTB.Core.FolderAccess
{
    public class BaseFolderManager
    {
        protected FolderSchema _schema;
        protected PTBSettings _settings;
        protected IPTBLogger _logger;

        public BaseFolderManager(PTBSettings settings, FolderSchema schema, IPTBLogger logger)
        {
            _schema = schema;
            _settings = settings;
            _logger = logger;
        }

        public bool IsMaskMatch(string fileName, string mask) => Regex.IsMatch(System.IO.Path.GetFileNameWithoutExtension(fileName), mask);

        public T GetFile<T>(string fileName) where T: BasePTBFile, new()
        {
            var path = System.IO.Path.Combine(_settings.HomeDirectory, _schema.Folder, fileName);
            object[] parameters = new object[] { _settings.FileDelimiter, _schema.LineSize, new System.IO.FileInfo(path) };
            T file = Activator.CreateInstance(typeof(T), parameters) as T;
            return file;
        }

        public PTBFolder<T> GetFolder<T>() where T: BasePTBFile, new()
        {
            var dirPath = System.IO.Path.Combine(_settings.HomeDirectory, _schema.Folder);
            var fileNames = System.IO.Directory.GetFiles(dirPath);

            List<T> files = new List<T>();

            foreach (var fileName in fileNames)
            {
                if (IsMaskMatch(fileName, _schema.FileMask)) {
                    var file = GetFile<T>(fileName);
                    files.Add(file);
                }
                else
                {
                    _logger.LogInfo($"Skipped file {fileName} because it didn't match the file mask");
                }
            }

            var folder = new PTBFolder<T>
            {
                DefaultFileName = _schema.DefaultFileName,
                Name = _schema.Folder,
                Files = files
            };

            return folder;
        }
    }
}
