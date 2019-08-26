using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PTB.Core.Logging;

namespace PTB.Web.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private PTBFileLogger _logger;

        public LoggingController(PTBFileLogger logger)
        {
            _logger = logger;
        }

        // POST: api/Logging/Log
        [HttpPost("[action]")]
        public void Log([FromBody] LogMessage logMessage)
        {
            _logger.Log(logMessage);
        }
    }
}