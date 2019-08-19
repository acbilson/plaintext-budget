using Microsoft.AspNetCore.Cors;
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
        // GET: api/Ledger?startIndex=0&count=10
        [HttpGet("[action]")]
        public List<Ledger> ReadLedgers(int startIndex, int count)
        {
            //var homeDirectory = Environment.CurrentDirectory;
            var homeDirectory = @"C:\Users\abilson\OneDrive - SPR Consulting\Archive\2019\BudgetProject\PTB_Home";
            var fileManager = new FileManager(homeDirectory);
            var client = new PTBClient();
            var logger = new PTBFileLogger(LoggingLevel.Debug, homeDirectory);
            client.Instantiate(fileManager, logger);
            var response = client.Ledger.ReadDefaultLedgerEntries(startIndex, count);
            return response.Result;
        }

        // GET: api/Ledger/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Ledger
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Ledger/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}