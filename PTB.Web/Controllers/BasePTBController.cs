using Microsoft.AspNetCore.Mvc;
using PTB.Core.Logging;
using System;

namespace PTB.Web.Controllers
{
    public class BasePTBController : ControllerBase
    {

        IPTBLogger _logger;

        public BasePTBController(IPTBLogger logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            long now = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var logMessage = new LogMessage(LoggingLevel.Debug, message, typeof(FolderController).Name, now.ToString());
            _logger.Log(logMessage);
        }

        public void LogError(string message)
        {
            long now = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var logMessage = new LogMessage(LoggingLevel.Error, message, typeof(FolderController).Name, now.ToString());
            _logger.Log(logMessage);
        }
    }
}