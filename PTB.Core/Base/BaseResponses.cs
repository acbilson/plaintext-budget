using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.Base
{

    public class BaseResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class StringToRowResponse : BaseResponse
    {
        public PTBRow Row { get; set; }
        public static StringToRowResponse Default => new StringToRowResponse { Success = true, Message = string.Empty, Row = new PTBRow() };
    }
    public class RowToStringResponse : BaseResponse
    {
        public string Line { get; set; }
        public static RowToStringResponse Default => new RowToStringResponse { Success = true, Message = string.Empty, Line = string.Empty };
    }


    public class BaseReadResponse : BaseResponse
    {
        public List<PTBRow> ReadResult { get; set; }
        public static BaseReadResponse Default => new BaseReadResponse { Success = true, Message = string.Empty, ReadResult = new List<PTBRow>() };
    }

    public class BaseUpdateResponse : BaseResponse
    {
        public static BaseUpdateResponse Default => new BaseUpdateResponse { Success = true, Message = string.Empty };
    }

    public class BaseAppendResponse : BaseResponse
    {
        public static BaseAppendResponse Default => new BaseAppendResponse { Success = true, Message = string.Empty };
    }



}
