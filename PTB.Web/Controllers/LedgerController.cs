using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PTB.Core.Base;
using PTB.Core.Exceptions;
using PTB.Core.Logging;
using PTB.Files.FolderAccess;
using PTB.Files.Ledger;
using PTB.Files.TitleRegex;
using System;
using System.Collections.Generic;
using System.Linq;

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

        // GET: api/Ledger/Read?fileName=checking&startIndex=0&count=10
        [HttpGet("[action]")]
        public List<PTBRow> Read(string fileName, int startIndex, int count)
        {
            var fileFolders = _fileFolderService.GetFileFolders();
            var ledgerFile = fileFolders.LedgerFolder.Files.First(file => file.ShortName == fileName);
            var response = _ledgerService.Read(ledgerFile, startIndex, count);

            if (!response.Success)
            {
                string message = $"Failed to read ledger entries from ledger {ledgerFile.ShortName}. Message was: {response.Message}";
                LogError(message);
                throw new WebException(message);
            }

            Log($"Read {response.ReadResult.Count} ledger entries from ledger {ledgerFile.ShortName}");
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
                string message = $"Failed to update ledger {defaultLedgerFile.ShortName} at index: {ledger.Index}. Message was {response.Message}";
                LogError(message);
                throw new WebException(message);
            }

            Log($"Updated ledger [index:{ledger.Index},subject:{ledger["subject"]},subcategory:{ledger["subcategory"]}]");
            return ledger;
        }

        private void LogError(string message)
        {
            long now = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var logMessage = new LogMessage(LoggingLevel.Error, message, nameof(LedgerController), now.ToString());
            _logger.Log(logMessage);
        }

        private void Log(string message)
        {
            long now = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var logMessage = new LogMessage(LoggingLevel.Debug, message, nameof(LedgerController), now.ToString());
            _logger.Log(logMessage);
        }
    }
}