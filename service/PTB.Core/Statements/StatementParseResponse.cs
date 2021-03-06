﻿namespace PTB.Core.Statements
{
    public class StatementParseResponse
    {
        public string Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static StatementParseResponse Default => new StatementParseResponse { Result = string.Empty, Success = true, Message = string.Empty };
    }
}