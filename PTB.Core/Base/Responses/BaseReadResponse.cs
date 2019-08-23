using System.Collections.Generic;

namespace PTB.Core.Base
{
    public class BaseReadResponse : BaseResponse
    {
        public List<PTBRow> ReadResult { get; set; }
        public static BaseReadResponse Default => new BaseReadResponse { Success = true, Message = string.Empty, ReadResult = new List<PTBRow>() };
    }
}