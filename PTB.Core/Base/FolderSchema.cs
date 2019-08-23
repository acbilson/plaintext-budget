using System.Collections.Generic;

namespace PTB.Core.Base
{
    public class FolderSchema
    {
        public string Folder { get; set; }
        public string Delimiter { get; set; }
        public string DefaultFileName { get; set; }
        public int LineSize { get; set; }
        public List<ColumnSchema> Columns { get; set; }
    }
}