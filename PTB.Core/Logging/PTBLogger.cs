using System;

namespace PTB.Core.Logging
{
    public interface IPTBLogger
    {
        void LogDebug(string message);

        void LogWarning(string message);

        void LogError(Exception exception);
    }

    public class PTBFileLogger : IPTBLogger
    {
        private System.IO.FileInfo _loggingFile;
        private LoggingLevel _level;

        public PTBFileLogger(LoggingLevel level, string baseDirectory)
        {
            _level = level;
            _loggingFile = new System.IO.FileInfo(System.IO.Path.Combine(baseDirectory, "ptb.log"));
        }

        private void Log(string message)
        {
            using (var writer = new System.IO.StreamWriter(_loggingFile.FullName, append: true))
            {
                writer.WriteLine(message);
            }
        }

        public void LogDebug(string message)
        {
            if (_level >= LoggingLevel.Debug)
            {
                Log($"DBUG: {message}");
            }
        }

        public void LogWarning(string message)
        {
            if (_level >= LoggingLevel.Warning)
            {
                Log($"WARN: {message}");
            }
        }

        public void LogError(Exception exception)
        {
            if (_level >= LoggingLevel.Error)
            {
                Log($"ERR: Message: {exception.ToString()}");
            }
        }
    }

    public enum LoggingLevel
    {
        Debug = 0,
        Warning = 1,
        Error = 2
    }
}