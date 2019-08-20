using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PTB.Core;
using PTB.Core.Ledger;
using PTB.Core.Logging;
using System;
using System.Collections.Generic;
using PTB.Core.PTBFileAccess;

namespace PTB.Web.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private PTBClient InstantiatePTBClient()
        {
            //var homeDirectory = Environment.CurrentDirectory;
            var homeDirectory = @"C:\Users\abilson\OneDrive - SPR Consulting\Archive\2019\BudgetProject\PTB_Home";
            var fileManager = new FileManager(homeDirectory);
            var client = new PTBClient();
            var logger = new PTBFileLogger(LoggingLevel.Debug, homeDirectory);
            client.Instantiate(fileManager, logger);
            return client;
        }
        // TODO: Return a new type that wraps the FileInfo object which can't be serialized
        // GET: api/File/GetLedgerFiles
        [HttpGet("[action]")]
        public List<PTBFile> GetLedgerFiles()
        {
            var client = InstantiatePTBClient();
            var ledgerFiles = client.FileManager.GetLedgerFiles();
            return ledgerFiles;
        }
    }
}