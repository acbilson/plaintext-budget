using System;

namespace PTB.Core.Logging
{
    public interface IPTBLogger
    {
        void Log(LogMessage logMessage);
        void LogInfo(string message);
        void LogDebug(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(Exception exception);

        void SetContext(string context);
        void SetLevel(LoggingLevel level);
    }

    public class PTBFileLogger
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

        public void Log(LogMessage logMessage)
        {
          string message = "";
          switch (logMessage.Level) {
            case LoggingLevel.Info: { message = $"INFO-{logMessage.Context} - {logMessage.Message}"; break; }
            case LoggingLevel.Debug: { message = $"DBUG-{logMessage.Context} - {logMessage.Message}"; break; }
            case LoggingLevel.Warning: { message = $"WARN-{logMessage.Context} - {logMessage.Message}"; break; }
            case LoggingLevel.Error: { message = $"ERR-{logMessage.Context} - {logMessage.Message}"; break; }
            }

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
                Log($"INFO-{_context}: {message}");
            }
        }

        public void LogDebug(string message)
        {
            if (_level <= LoggingLevel.Debug)
            {
                Log($"DBUG-{_context}: {message}");
            }
        }

        public void LogWarning(string message)
        {
            if (_level <= LoggingLevel.Warning)
            {
                Log($"WARN-{_context}: {message}");
            }
        }

        public void LogError(string message)
        {
            if (_level <= LoggingLevel.Error)
            {
                Log($"ERR-{_context}: {message}");
            }
        }

        public void LogError(Exception exception)
        {
            if (_level <= LoggingLevel.Error)
            {
                Log($"ERR-{_context}: Message: {exception.ToString()}");
            }
        }
    }

    public enum LoggingLevel
    {
        Info = 0,
        Debug = 1,
        Warning = 2,
        Error = 3
    }
}
