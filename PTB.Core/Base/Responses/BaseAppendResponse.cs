namespace PTB.Core.Base
{
    public class BaseAppendResponse : BaseResponse
    {
        public static BaseAppendResponse Default => new BaseAppendResponse { Success = true, Message = string.Empty };
    }
}