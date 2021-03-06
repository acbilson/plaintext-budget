﻿namespace PTB.Core.Base
{
    public class PTBColumn : ColumnSchema
    {
        private string _columnValue;

        public virtual string ColumnValue
        {
            get { return LengthExceedsSize(_columnValue) ? _columnValue.Trim().Substring(0, Size) : new string(' ', Size - _columnValue.Trim().Length) + _columnValue.Trim(); }
            set { _columnValue = value; }
        }

        public PTBColumn()
        {
        }

        public PTBColumn(ColumnSchema schema)
        {
            base.Size = schema.Size;
            base.ColumnName = schema.ColumnName;
            base.Index = schema.Index;
            base.Offset = schema.Offset;
            base.Editable = schema.Editable;
        }

        public bool LengthExceedsSize(string value) => value.Trim().Length > Size;

        public override string ToString()
        {
            var builder = new System.Text.StringBuilder();
            builder.Append("( Name: ");
            builder.Append(ColumnName);
            builder.Append(", ");
            builder.Append("Value: ");
            builder.Append(ColumnValue.Trim());
            builder.Append(", ");
            builder.AppendLine($"I:{Index} O:{Offset}, S:{Size}, Edit:{Editable} )");
            return builder.ToString();
        }
    }
}