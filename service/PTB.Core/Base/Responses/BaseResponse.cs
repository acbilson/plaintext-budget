namespace PTB.Core.Base
{
    public class BaseResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }

        public static BaseResponse Default => new BaseResponse { Success = true, Message = string.Empty };
    }
}