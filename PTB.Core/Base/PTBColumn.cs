namespace PTB.Core.Base
{
    public class PTBColumn : ColumnSchema
    {
        private string _columnValue;

        public string ColumnValue
        {
            get { return _columnValue; }
            set { _columnValue = LengthExceedsSize(value) ? value.Trim().Substring(0, Size) : new string(' ', Size - value.Trim().Length) + value.Trim(); }
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
        }

        private bool LengthExceedsSize(string value) => value.Trim().Length > Size;
    }
}