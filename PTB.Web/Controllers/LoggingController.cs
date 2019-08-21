using Microsoft.AspNetCore.Mvc;
using PTB.Core;
using PTB.Core.Ledger;
using PTB.Core.Logging;
using System;
using System.Collections.Generic;

namespace PTB.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private PTBClient InstantiatePTBClient()
        {
            //var homeDirectory = Environment.CurrentDirectory;
            var homeDirectory = @"C:\Users\abilson\OneDrive - SPR Consulting\Archive\2019\BudgetProject\PTB_Home";
            var fileManager = new FileManager(homeDirectory);
            var logger = PTBFileLogger.Instance;
            logger.Configure(LoggingLevel.Debug, homeDirectory);
            var client = PTBClient.Instance;
            client.Instantiate(fileManager, logger);
            return client;
        }
        // GET: api/ReadLedgers?startIndex=0&count=10
        [HttpGet("[action]")]
        public List<Ledger> ReadLedgers(int startIndex, int count)
        {
            var client = InstantiatePTBClient();
            var response = client.Ledger.ReadDefaultLedgerEntries(startIndex, count);
            return response.Result;
        }

        // POST: api/Logging/Log
        [HttpPost("[action]")]
        public void Log([FromBody] LogMessage logMessage)
        {
            var homeDirectory = @"C:\Users\abilson\OneDrive - SPR Consulting\Archive\2019\BudgetProject\PTB_Home";
            var logger = PTBFileLogger.Instance;
            logger.Configure(LoggingLevel.Debug, homeDirectory);
            logger.Log(logMessage);
        }
    }
}
