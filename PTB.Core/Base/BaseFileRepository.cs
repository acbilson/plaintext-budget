using Newtonsoft.Json;
using System.Linq;

namespace PTB.Core
{
    public class BaseFileRepository
    {
        protected PTBSettings _settings;
        protected PTBSchema _schema;
        protected FileManager _fileManager;

        public BaseFileRepository(FileManager fileManager)
        {
            _fileManager = fileManager;
            _settings = _fileManager.Settings;
            _schema = _fileManager.Schema;
        }

        public BaseFileRepository(PTBSettings settings, PTBSchema schema)
        {
            _settings = settings;
            _schema = schema;
        }

        public bool HasByteOrderMark(byte[] buffer) => buffer[0] == 239;

        // Windows new line has both byte 13 (\n) and byte 10 (\r). Unix only has byte 13 (\n), so it will not contain byte 10 (\r)
        public bool HasUnixNewLine(byte[] buffer) => buffer.Any((b) => b == 10) == false;

        public long GetLineNumber(long streamPosition, int lineSize) => streamPosition / (lineSize + System.Environment.NewLine.Length);

        public PTBSchema ReadFileSchema(string home)
        {
            string path = System.IO.Path.Combine(home, "schema.json");
            string text = System.IO.File.ReadAllText(path);
            PTBSchema schema = JsonConvert.DeserializeObject<PTBSchema>(text);
            return schema;
        }

        public string GetDefaultPath(string folder, string name)
        {
            return System.IO.Path.Combine(_settings.HomeDirectory, folder, (name + _settings.FileExtension));
        }

        public string GetCopyPath(string folder, string name)
        {
            return System.IO.Path.Combine(_settings.HomeDirectory, folder, (name + "-copy" + _settings.FileExtension));
        }
    }

    public enum FileType
    {
        Ledger,
        Categories
    }
}