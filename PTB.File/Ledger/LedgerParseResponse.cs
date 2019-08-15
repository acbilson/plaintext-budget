namespace PTB.File.Ledger
{
    public class LedgerToStringResponse
    {
        public string Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static LedgerToStringResponse Default => new LedgerToStringResponse { Result = string.Empty, Success = true, Message = string.Empty };
    }

    public class StringToLedgerResponse
    {
        public Ledger Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static StringToLedgerResponse Default => new StringToLedgerResponse { Success = true, Message = string.Empty };
    }
}