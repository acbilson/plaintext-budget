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
        private BaseFileManager _fileManager;

        public FileController(PTBFileLogger logger, BaseFileManager fileManager)
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
        public List<BasePTBFile> GetLedgerFiles()
        {
            var client = InstantiatePTBClient();
            var ledgerFiles = client.FileManager.GetLedgerFiles();
            Log($"Retrieved {ledgerFiles.Count} ledger files from folder");
            return ledgerFiles;
        }

        private void Log(string message)
        {
            var logMessage = new LogMessage(LoggingLevel.Debug, message, typeof(FileController).Name);
            _logger.Log(logMessage);
        }
    }
}