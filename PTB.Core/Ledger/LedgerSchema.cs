using PTB.Core.Base;
using System.Linq;

namespace PTB.Core.Ledger
{
    public class LedgerSchema : FolderSchema
    {
        public string FileMask { get; set; }
        public LedgerColumns Columns { get; set; }
    }

    public class LedgerColumns
    {
        public ColumnDate Date { get; set; }
        public ColumnType Type { get; set; }
        public ColumnAmount Amount { get; set; }
        public ColumnSubcategory Subcategory { get; set; }
        public ColumnSubject Subject { get; set; }
        public ColumnTitle Title { get; set; }
        public ColumnLocked Locked { get; set; }
    }

    public class ColumnDate : SchemaColumn { }

    public class ColumnType : SchemaColumn { }

    public class ColumnAmount : SchemaColumn { }

    public class ColumnSubcategory : SchemaColumn { }

    public class ColumnSubject : SchemaColumn { }

    public class ColumnTitle : SchemaColumn { }

    public class ColumnLocked : SchemaColumn { }
}