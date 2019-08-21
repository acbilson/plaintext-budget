namespace PTB.Core.Logging
{
  public struct LogMessage {

    public LoggingLevel Level;
    public string Message;
    public string Context;

    public LogMessage(LoggingLevel level, string message, string context) {
      Level = level;
      Message = message;
      Context = context;
    }
  }
}
