using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTB.Core.Base;
using PTB.Core.Ledger;
using PTB.Core.TitleRegex;
using Newtonsoft.Json;

namespace PTB.Core.Tests
{
    [TestClass]
    public class GlobalSetup
    {
        public PTBSchema Schema;

        [TestInitialize]
        public void Initialize()
        {
            Schema = GetDefaultSchema();
        }

        public PTBSchema GetDefaultSchema()
        {
            var text = System.IO.File.ReadAllText("./Config/Clean/schema.json");
            PTBSchema schema = JsonConvert.DeserializeObject<PTBSchema>(text);
            return schema;
        }
    }
}