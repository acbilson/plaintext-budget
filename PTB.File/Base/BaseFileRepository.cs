using System;
using System.Collections.Generic;
using System.Text;
using PTB.File.Ledger;
using PTB.Core;
using Newtonsoft.Json;
using System.IO;

namespace PTB.File
{
    public class BaseFileRepository
    {
        public LedgerSchema ReadFileSchema(string home, string folder)
        {
            string path = System.IO.Path.Combine(home, folder, "schema.json");
            string text = System.IO.File.ReadAllText(path);
            LedgerSchema schema = JsonConvert.DeserializeObject<LedgerSchema>(text);
            return schema;
        }

        public string GetDefaultPath(string home, string folder, string name)
        {
            return System.IO.Path.Combine(home, folder, (name + Constant.FILE_EXTENSION));
        }
    }

    public enum FileType
    {
        Ledger,
        Categories
    }
}
