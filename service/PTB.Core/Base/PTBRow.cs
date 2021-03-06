﻿using PTB.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTB.Core.Base
{
    public class PTBRow
    {
        public int Index { get; set; }
        public List<PTBColumn> Columns { get; set; }

        public PTBRow()
        {
            Columns = new List<PTBColumn>();
        }

        private bool NameEquals(string columnName, string value) => columnName.Equals(value, StringComparison.OrdinalIgnoreCase);

        private bool MissingColumn(string name) => !Columns.Exists(column => NameEquals(column.ColumnName, name));

        public string this[string columnName]
        {
            get
            {
                return GetValueByName(columnName);
            }
            set
            {
                SetValueByName(columnName, value);
            }
        }

        private string GetValueByName(string name)
        {
            if (MissingColumn(name)) { throw new ParseException($"Row contains no column with name: {name}"); }
            return Columns.First(column => NameEquals(column.ColumnName, name)).ColumnValue;
        }

        private void SetValueByName(string name, string value)
        {
            if (MissingColumn(name)) { throw new ParseException($"Row contains no column with name: {name}"); }
            Columns.ForEach(column =>
            {
                if (NameEquals(column.ColumnName, name))
                {
                    column.ColumnValue = value;
                }
            });
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Index: ");
            builder.AppendLine(Index.ToString());
            builder.AppendLine("Columns: [");
            foreach (var column in Columns)
            {
                builder.Append(column.ToString());
            }
            builder.AppendLine("]");
            return builder.ToString();
        }
    }
}