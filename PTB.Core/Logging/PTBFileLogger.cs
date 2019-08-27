using System;

namespace PTB.Core.Logging
{
    public class PTBFileLogger : IPTBLogger
    {
        private System.IO.FileInfo _loggingFile;
        private LoggingLevel _level;
        private string _context = "NA";

        public PTBFileLogger(LoggingLevel level, string baseDirectory)
        {
            _level = level;
            _loggingFile = new System.IO.FileInfo(System.IO.Path.Combine(baseDirectory, "ptb.log"));
            System.IO.File.Delete(_loggingFile.FullName);
        }

        private long GetTimestamp() => (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;

        private string GetLoggingString(LogMessage msg)
        {
            string lvl = msg.Level.ToString();
            return $"{msg.Timestamp}-{lvl}-{msg.Context}: {msg.Message}";
        }

        public void Log(LogMessage logMessage)
        {
            string message = GetLoggingString(logMessage);
            this.Log(message);
        }

        private void Log(string message)
        {
            using (var writer = new System.IO.StreamWriter(_loggingFile.FullName, append: true))
            {
                writer.WriteLine(message);
            }
        }

        public void SetContext(string context)
        {
            _context = context;
        }

        public void SetLevel(LoggingLevel level)
        {
            _level = level;
        }

        public void LogInfo(string message)
        {
            if (_level <= LoggingLevel.Info)
            {
                var logMessage = new LogMessage(LoggingLevel.Info, message, _context, GetTimestamp().ToString());
                Log(logMessage);
            }
        }

        public void LogDebug(string message)
        {
            if (_level <= LoggingLevel.Debug)
            {
                var logMessage = new LogMessage(LoggingLevel.Debug, message, _context, GetTimestamp().ToString());
                Log(logMessage);
            }
        }

        public void LogWarning(string message)
        {
            if (_level <= LoggingLevel.Warning)
            {
                var logMessage = new LogMessage(LoggingLevel.Warning, message, _context, GetTimestamp().ToString());
                Log(logMessage);
            }
        }

        public void LogError(string message)
        {
            if (_level <= LoggingLevel.Error)
            {
                var logMessage = new LogMessage(LoggingLevel.Error, message, _context, GetTimestamp().ToString());
                Log(logMessage);
            }
        }

        public void LogError(Exception exception)
        {
            if (_level <= LoggingLevel.Error)
            {
                var logMessage = new LogMessage(LoggingLevel.Error, exception.ToString(), _context, GetTimestamp().ToString());
                Log(logMessage);
            }
        }
    }
}