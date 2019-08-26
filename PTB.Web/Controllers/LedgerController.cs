using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PTB.Core;
using PTB.Core.Base;
using PTB.Core.Exceptions;
using PTB.Core.Logging;
using PTB.Files.FolderAccess;
using PTB.Files.Ledger;
using PTB.Files.TitleRegex;
using System;
using System.Collections.Generic;

namespace PTB.Web.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class LedgerController : ControllerBase
    {
        private IPTBLogger _logger;
        private FileFolderService _fileFolderService;
        private LedgerService _ledgerService;
        private TitleRegexService _titleRegexService;

        public LedgerController(FileFolderService fileFolderService, LedgerService ledgerService, TitleRegexService titleRegexService, IPTBLogger logger)
        {
            _fileFolderService = fileFolderService;
            _ledgerService = ledgerService;
            _titleRegexService = titleRegexService;
            _logger = logger;
        }

        // GET: api/Ledger/Read?startIndex=0&count=10
        [HttpGet("[action]")]
        public List<PTBRow> Read(int startIndex, int count)
        {
            var fileFolders = _fileFolderService.GetFileFolders();
            var defaultLedgerFile = fileFolders.LedgerFolder.GetDefaultFile();
            var response = _ledgerService.Read(defaultLedgerFile, startIndex, count);

            if (!response.Success)
            {
                string message = $"Failed to read ledger entries from ledger {defaultLedgerFile.FileName}. Message was: {response.Message}";
                LogError(message);
                throw new WebException(message);
            }

            Log($"Read {response.ReadResult.Count} ledger entries from the ledger {defaultLedgerFile.FileName}");
            return response.ReadResult;
        }

        // GET: api/Ledger/Update
        [HttpPut("[action]")]
        public PTBRow Update([FromBody] PTBRow ledger)
        {
            var fileFolders = _fileFolderService.GetFileFolders();
            var defaultLedgerFile = fileFolders.LedgerFolder.GetDefaultFile();
            var response = _ledgerService.Update(defaultLedgerFile, ledger.Index, ledger);

            if (!response.Success)
            {
                string message = $"Failed to update ledger {defaultLedgerFile.FileName} at index: {ledger.Index}. Message was {response.Message}";
                LogError(message);
                throw new WebException(message);
            }

            Log($"Updated ledger with index: {ledger.Index}");
            return ledger;
        }

        private void LogError(string message)
        {
            var logMessage = new LogMessage(LoggingLevel.Error, message, typeof(FolderController).Name);
            _logger.Log(logMessage);
        }
        private void Log(string message)
        {
            var logMessage = new LogMessage(LoggingLevel.Debug, message, typeof(FolderController).Name);
            _logger.Log(logMessage);
        }

    }
}
