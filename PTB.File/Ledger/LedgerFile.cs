using System;
using System.Collections.Generic;
using System.IO;

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

        #endregion IDisposable Support
    }
}