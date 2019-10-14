namespace PTB.Core.Base
{
    public class RowToStringResponse : BaseResponse
    {
        public string Line { get; set; }
        public new static RowToStringResponse Default => new RowToStringResponse { Success = true, Message = string.Empty, Line = string.Empty };
    }
}