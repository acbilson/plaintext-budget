using Newtonsoft.Json;
using PTB.Core.Base;

namespace PTB.Core
{
    public class PTBClient
    {
        private string _baseDirectory;

        public PTBClient(string baseDirectory)
        {
            _baseDirectory = baseDirectory;
        }
    }
}