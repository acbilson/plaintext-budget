using PTB.File.Ledger;
using PTB.File.TitleRegex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace PTB.File.Ledger
{
    public class LedgerFile : IDisposable
    {

        private string _Path;
        private StreamWriter _writer;

        public LedgerFile(string path)
        {
            _Path = path;
            _writer = new StreamWriter(_Path);
        }

        public IEnumerable<string> ReadLine()
        {
            using (var reader = new StreamReader(_Path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public void WriteLine(string line)
        {
            _writer.WriteLine(line);

        }

        public void Categorize(LedgerParser parser, int bufferSize, TitleRegex.TitleRegex[] categories)
        {
            using (var stream = System.IO.File.Open(_Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                var reader = new StreamReader(stream, Encoding.UTF8);
                var writer = new StreamWriter(stream, Encoding.UTF8);

                char[] buffer = new char[bufferSize];
                while (reader.Read(buffer, 0, buffer.Length) != 0)
                {
                    Ledger current = parser.ParseLine(buffer.ToString());

                    foreach (var category in categories)
                    {
                        if (Regex.IsMatch(current.Title, category.Regex))
                        {
                            current.Subcategory = category.Subcategory;

                            // returns stream to overwrite
                            stream.Position -= buffer.Length;
                            writer.Write(parser.ParseLedger(current));

                            // moves to next line
                            continue;
                        }
                    }
                }
            }

        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _writer.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
