using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.Base
{
    public class PTBRow
    {
        public List<PTBColumn> Columns { get; set; }
    }

    public class PTBColumn : ColumnSchema
    {
        public PTBColumn() { }

        public PTBColumn(ColumnSchema schema)
        {
            base.Size = schema.Size;
            base.ColumnName = schema.ColumnName;
            base.Index = schema.Index;
            base.Offset = schema.Offset;
        }

        private string _columnValue;
        public string ColumnValue {
            get { return _columnValue; }
            set { _columnValue = new string(' ', Size - value.Trim().Length) + value; }
        }
    }
}
