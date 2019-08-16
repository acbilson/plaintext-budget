using PTB.Core.Base;
using System.Linq;

namespace PTB.Core.TitleRegex
{
    public class TitleRegexSchema : FolderSchema
    {
        public TitleRegexColumns Columns { get; set; }
    }

    public class TitleRegexColumns
    {
        public ColumnPriority Priority { get; set; }
        public ColumnSubcategory Subcategory { get; set; }
        public ColumnRegex Regex { get; set; }
        public ColumnSubject Subject { get; set; }
    }

    public class ColumnPriority : SchemaColumn { }

    public class ColumnSubcategory : SchemaColumn { }

    public class ColumnRegex : SchemaColumn { }
    public class ColumnSubject : SchemaColumn { }
}