using PTB.Core.Base;
using PTB.Core.Exceptions;
using PTB.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PTB.Core.FolderAccess
{
    public class BaseFolderService
    {
        protected FolderSchema _schema;
        protected PTBSettings _settings;
        protected IPTBLogger _logger;

        public BaseFolderService(PTBSettings settings, FolderSchema schema, IPTBLogger logger)
        {
            _schema = schema;
            _settings = settings;
            _logger = logger;
            _logger.SetContext(nameof(BaseFolderService));
        }

        public bool IsMaskMatch(string fileName, string mask) => Regex.IsMatch(System.IO.Path.GetFileNameWithoutExtension(fileName), mask);

        public bool FileSizeIsIndivisibleByLineLength(long fileLength, int lineSize) => fileLength % lineSize != 0;

        public bool FileIsEmpty(long fileLength) => fileLength == 0;

        public bool FileIsThreeBytes(long fileLength) => fileLength == 3;

        public T GetFile<T>(string fileName) where T : BasePTBFile, new()
        {
            var path = System.IO.Path.Combine(_settings.HomeDirectory, _schema.Folder, fileName);
            var fileInfo = new System.IO.FileInfo(path);

            if (FileIsEmpty(fileInfo.Length))
            {
                string message = $"The file {fileName} is empty.";
                _logger.LogWarning(message);
            }
            else if (FileIsThreeBytes(fileInfo.Length))
            {
                string message = $"The file {fileName} has only three bytes, which may indicate an empty file with a UTF-8 byte order mark.";
                _logger.LogWarning(message);
            }
            else if (FileSizeIsIndivisibleByLineLength(fileInfo.Length, _schema.LineSize + Environment.NewLine.Length))
            {
                string message = $"The file {fileName} has a byte length that is indivisible by the schema line size. This may indicate that the file has been corrupted";
                _logger.LogError(message);
                throw new FileException(message);
            }

            object[] parameters = new object[] { _settings.FileDelimiter, _schema.LineSize, fileInfo };
            T file = Activator.CreateInstance(typeof(T), parameters) as T;
            return file;
        }

        public PTBFolder<T> GetFolder<T>() where T : BasePTBFile, new()
        {
            var dirPath = System.IO.Path.Combine(_settings.HomeDirectory, _schema.Folder);
            var fileNames = System.IO.Directory.GetFiles(dirPath);

            List<T> files = new List<T>();

            foreach (var fileName in fileNames)
            {
                if (IsMaskMatch(fileName, _schema.FileMask))
                {
                    var file = GetFile<T>(fileName);
                    files.Add(file);
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