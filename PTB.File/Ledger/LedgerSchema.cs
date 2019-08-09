using System.Linq;
using PTB.File.Base;

namespace PTB.File.Ledger
{
    public class LedgerSchema
    {
        // from schema.json
        public string Delimiter { get; set; }

        public LedgerColumns Columns { get; set; }
        public SchemaFiles[] Files { get; set; }

        public string GetDefaultName()
        {
            return Files.First((l) => l.IsDefault == true).Name;
        }
    }

    public class LedgerColumns
    {
        public ColumnDate Date { get; set; }
        public ColumnType Type { get; set; }
        public ColumnAmount Amount { get; set; }
        public ColumnSubcategory Subcategory { get; set; }
        public ColumnTitle Title { get; set; }
        public ColumnLocation Location { get; set; }
        public ColumnLocked Locked { get; set; }
    }


    public class ColumnDate : SchemaColumn { }

    public class ColumnType : SchemaColumn { }

    public class ColumnAmount : SchemaColumn { }

    public class ColumnSubcategory : SchemaColumn { }

    public class ColumnTitle : SchemaColumn { }

    public class ColumnLocation : SchemaColumn { }

    public class ColumnLocked : SchemaColumn { }
}