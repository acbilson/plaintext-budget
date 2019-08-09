using PTB.File.Base;
using System.Linq;

namespace PTB.File.TitleRegex
{
    public class TitleRegexSchema
    {
        // from schema.json
        public string Delimiter { get; set; }

        public int Size { get; set; }
        public TitleRegexColumns Columns { get; set; }
        public PTBFiles[] Files { get; set; }

        public string GetDefaultName()
        {
            return Files.First((l) => l.IsDefault == true).Name;
        }
    }

    public class TitleRegexColumns
    {
        public ColumnPriority Priority { get; set; }
        public ColumnSubcategory Subcategory { get; set; }
        public ColumnRegex Regex { get; set; }
    }

    public class ColumnPriority : SchemaColumn { }

    public class ColumnSubcategory : SchemaColumn { }

    public class ColumnRegex : SchemaColumn { }
}