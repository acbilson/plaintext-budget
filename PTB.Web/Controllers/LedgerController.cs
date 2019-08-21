﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PTB.Core;
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
        private PTBClient InstantiatePTBClient()
        {
            //var homeDirectory = Environment.CurrentDirectory;
            var homeDirectory = @"C:\Users\abilson\OneDrive - SPR Consulting\Archive\2019\BudgetProject\PTB_Home";
            var fileManager = new FileManager(homeDirectory);
            var client = PTBClient.Instance;
            var logger = PTBFileLogger.Instance;
            logger.Configure(LoggingLevel.Debug, homeDirectory);
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

        // GET: api/UpdateLedger
        [HttpPut("[action]")]
        public Ledger UpdateLedger([FromBody] Ledger ledger)
        {
            var client = InstantiatePTBClient();
            var response = client.Ledger.UpdateDefaultLedgerEntry(ledger);
            if (!response.Success)
            {
                throw new Exception("Failed to update ledger");
            }
            return ledger;
        }
    }
}
