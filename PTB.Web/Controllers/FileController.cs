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
        private PTBFileLogger _logger;
        private FileManager _fileManager;

        public FileController(PTBFileLogger logger, FileManager fileManager)
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
        // GET: api/File/GetLedgerFiles
        [HttpGet("[action]")]
        public List<PTBFile> GetLedgerFiles()
        {
            var client = InstantiatePTBClient();
            var ledgerFiles = client.FileManager.GetLedgerFiles();
            _logger.LogDebug($"Retrieved {ledgerFiles.Count} ledger files from folder");
            return ledgerFiles;
        }
    }
}