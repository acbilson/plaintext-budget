using PTB.Core.Base;

namespace PTB.Core.Reports
{
    public class PTBReportColumn : PTBColumn
    {
        // overrides string prepend to append instead for reporting purposes
        private string _columnValue;
        public override string ColumnValue {
            get { return LengthExceedsSize(_columnValue) ? _columnValue.Trim().Substring(0, Size) : _columnValue.Trim() + new string(' ', Size - _columnValue.Trim().Length); }
            set {_columnValue = value; }
        }

        public PTBReportColumn() { }
        public PTBReportColumn(ColumnSchema schema): base(schema) { }
    }
}