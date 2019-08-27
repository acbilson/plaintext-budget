namespace PTB.Core.Logging
{
    public struct LogMessage
    {
        public string Timestamp;
        public LoggingLevel Level;
        public string Message;
        public string Context;

        public LogMessage(LoggingLevel level, string message, string context, string timestamp)
        {
            Level = level;
            Message = message;
            Context = context;
            Timestamp = timestamp;
        }
    }
}