namespace PTB.Core.Base
{
    public class ColumnSchema
    {
        public string ColumnName { get; set; }
        public int Index { get; set; }
        public int Size { get; set; }
        public int Offset { get; set; }
        public bool Editable { get; set; }
    }
}