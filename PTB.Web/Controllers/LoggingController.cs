﻿using Microsoft.AspNetCore.Mvc;
using PTB.Core;
using PTB.Core.Logging;

namespace PTB.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private PTBFileLogger _logger;
        private BaseFileManager _fileManager;

        public LoggingController(PTBFileLogger logger, BaseFileManager fileManager)
        {
            _logger = logger;
            _fileManager = fileManager;
        }

        // POST: api/Logging/Log
        [HttpPost("[action]")]
        public void Log([FromBody] LogMessage logMessage)
        {
            _logger.Log(logMessage);
        }
    }
}
