using Newtonsoft.Json;

namespace PTB.File
{
    public class BaseFileRepository
    {
        protected PTBSettings _settings;
        protected PTBSchema _schema;

        public BaseFileRepository(PTBSettings settings, PTBSchema schema)
        {
            _settings = settings;
            _schema = schema;
        }

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