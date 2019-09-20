namespace PTB.Core.Base
{
    public class StringToRowResponse : BaseResponse
    {
        public PTBRow Row { get; set; }
        public new static StringToRowResponse Default => new StringToRowResponse { Success = true, Message = string.Empty, Row = new PTBRow() };
    }
}