namespace PTB.Core.Base
{
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

        private bool LengthExceedsSize(string value) => value.Trim().Length > Size;

        public string ColumnValue
        {
            get { return _columnValue; }
            set { _columnValue = LengthExceedsSize(value) ? value.Trim().Substring(0, Size) : new string(' ', Size - value.Trim().Length) + value.Trim(); }
        }
    }
}