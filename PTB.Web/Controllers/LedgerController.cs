using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTB.File;
using PTB.File.Ledger;

namespace PTB.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LedgerController : ControllerBase
    {
        // GET: api/Ledger?startIndex=0&count=10
        [HttpGet("[action]")]
        public List<Ledger> ReadLedgers(int startIndex, int count)
        {
            var home = Environment.CurrentDirectory;
            var client = new FileClient();
            client.Instantiate(home);
            var ledgerEntries = client.Ledger.ReadDefaultLedgerEntries(startIndex, count);
            return ledgerEntries;
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
