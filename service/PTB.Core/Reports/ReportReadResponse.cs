using PTB.Core.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PTB.Core.Exceptions;

namespace PTB.Core.Reports
{
    public class ReportReadResponse : BaseReadResponse
    {
        public PTBRow GetRowBySubcategoryValue(string subcategoryValue)
        {
            if (base.ReadResult == null) throw new ParseException("No report read results to retrieve a row");

            var row = base.ReadResult.First(r => !IsSectionHeader(r.Columns) && HasSubcategoryValue(r, subcategoryValue));

            return row;
        }
        public new static ReportReadResponse Default => new ReportReadResponse { Success = true, Message = string.Empty, ReadResult = new List<PTBRow>() };

        protected bool IsSectionHeader(List<PTBColumn> columns) => columns.Any(c => ((ReportColumn)c).IsHeaderColumn == true);

        protected bool HasSubcategoryValue(PTBRow row, string subcategoryValue) => row["Subcategory"].TrimEnd() == subcategoryValue;
    }
}
