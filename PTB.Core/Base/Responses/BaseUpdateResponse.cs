namespace PTB.Core.Base
{
    public class BaseUpdateResponse : BaseResponse
    {
        public new static BaseUpdateResponse Default => new BaseUpdateResponse { Success = true, Message = string.Empty };
    }
}