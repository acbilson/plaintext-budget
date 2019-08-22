using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PTB.Core;
using PTB.Core.Exceptions;
using PTB.Core.Ledger;
using PTB.Core.Logging;
using System;
using System.Collections.Generic;

namespace PTB.Web.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class LedgerController : ControllerBase
    {
        private PTBFileLogger _logger;
        private BaseFileManager _fileManager;

        public LedgerController(PTBFileLogger logger, BaseFileManager fileManager)
        {
            _logger = logger;
            _fileManager = fileManager;
        }

        private PTBClient InstantiatePTBClient()
        {
            var client = PTBClient.Instance;
            client.Instantiate(_fileManager, _logger);
            return client;
        }
        // GET: api/ReadLedgers?startIndex=0&count=10
        [HttpGet("[action]")]
        public List<Ledger> ReadLedgers(int startIndex, int count)
        {
            var client = InstantiatePTBClient();
            var response = client.Ledger.ReadDefaultLedgerEntries(startIndex, count);
            Log($"Read {response.Result.Count} ledger entries from the default ledger");
            return response.Result;
        }

        // GET: api/UpdateLedger
        [HttpPut("[action]")]
        public Ledger UpdateLedger([FromBody] Ledger ledger)
        {
            var client = InstantiatePTBClient();
            var response = client.Ledger.UpdateDefaultLedgerEntry(ledger);

            if (!response.Success)
            {
                string message = $"Failed to update ledger with index: {ledger.Index}";
                LogError(message);
                throw new WebException(message);
            }

            Log($"Updated ledger with index: {ledger.Index}");
            return ledger;
        }

        private void LogError(string message)
        {
            var logMessage = new LogMessage(LoggingLevel.Error, message, typeof(FileController).Name);
            _logger.Log(logMessage);
        }
        private void Log(string message)
        {
            var logMessage = new LogMessage(LoggingLevel.Debug, message, typeof(FileController).Name);
            _logger.Log(logMessage);
        }

    }
}
