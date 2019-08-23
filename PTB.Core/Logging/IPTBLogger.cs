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
}