using System.Collections.Generic;

namespace PTB.File.TitleRegex
{
    public class TitleRegexRepository : BaseFileRepository
    {
        private string _folder = "Regex";
        private TitleRegexParser _parser;

        public TitleRegexRepository(PTBSettings settings, PTBSchema schema) : base(settings, schema)
        {
            _parser = new TitleRegexParser(_schema.Regex);
        }

        public IEnumerable<PTB.File.TitleRegex.TitleRegex> ReadAllTitleRegex()
        {
            var titleRegexs = new List<TitleRegex>();
            string path = base.GetDefaultPath(_folder, _schema.Regex.GetDefaultName());
            TitleRegexFile titleRegexFile = new TitleRegexFile(path);

            foreach (string line in titleRegexFile.ReadLine())
            {
                titleRegexs.Add(_parser.ParseLine(line));
            }
            return titleRegexs;
        }
    }
}