using System.Collections.Generic;
using System.IO;

namespace PTB.Core.TitleRegex
{
    public class TitleRegexFile
    {
        private string _path;

        public TitleRegexFile(string path)
        {
            _path = path;
        }

        public IEnumerable<string> ReadLine()
        {
            using (var reader = new StreamReader(_path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}