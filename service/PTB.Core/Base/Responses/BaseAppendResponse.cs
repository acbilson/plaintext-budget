namespace PTB.Core.Base
{
    public class BaseAppendResponse : BaseResponse
    {
        public new static BaseAppendResponse Default => new BaseAppendResponse { Success = true, Message = string.Empty };
    }
}