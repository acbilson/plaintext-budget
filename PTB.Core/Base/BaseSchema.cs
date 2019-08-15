namespace PTB.Core.Base
{
    public class FolderSchema
    {
        public string Folder { get; set; }
        public string Delimiter { get; set; }
        public string DefaultFileName { get; set; }
        public int LineSize { get; set; }
    }

    public class SchemaColumn
    {
        // from schema.json
        public int Index { get; set; }

        public int Size { get; set; }
        public int Offset { get; set; }
    }

    public class PTBFiles
    {
        // from schema.json
        public bool IsDefault { get; set; }

        public string Name { get; set; }
    }
}