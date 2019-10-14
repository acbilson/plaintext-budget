using PTB.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PTB.Core.Base
{
    public class FolderSchema
    {
        public string FileMask { get; set; }
        public string Folder { get; set; }
        public string Delimiter { get; set; }
        public string DefaultFileName { get; set; }
        public int LineSize { get; set; }
        public List<ColumnSchema> Columns { get; set; }

        private bool NameEquals(string columnName, string value) => columnName.Equals(value, StringComparison.OrdinalIgnoreCase);

        private bool MissingColumn(string name) => !Columns.Exists(column => NameEquals(column.ColumnName, name));

        private ColumnSchema GetColumnByName(string name)
        {
            if (MissingColumn(name)) { throw new ParseException($"Folder schema contains no column with name: {name}"); }
            return Columns.First(column => NameEquals(column.ColumnName, name));
        }

        public ColumnSchema this[string columnName]
        {
            get
            {
                return GetColumnByName(columnName);
            }
        }
    }
}