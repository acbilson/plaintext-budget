using Newtonsoft.Json;
using PTB.Core.Categories;
using PTB.Core.Budget;
using PTB.Core.Ledger;
using PTB.Core.TitleRegex;
using PTB.Core.Logging;
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