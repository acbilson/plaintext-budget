using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PTB.Core;
using PTB.Core.Logging;
using System;
using System.Collections.Generic;
using PTB.Core.FolderAccess;
using PTB.Files.Ledger;
using PTB.Files.FolderAccess;
using PTB.Core.Base;

namespace PTB.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private IPTBLogger _logger;
        private FileFolderService _fileFolderService;

        public FolderController(FileFolderService fileFolderService, IPTBLogger logger)
        {
            _logger = logger;
            _fileFolderService = fileFolderService;
        }

        // GET: api/File/GetFileFolders
        [HttpGet("[action]")]
        public FileFolders GetFileFolders()
        {
            var folders = _fileFolderService.GetFileFolders();
            return folders;
        }

        private void Log(string message)
        {
            var logMessage = new LogMessage(LoggingLevel.Debug, message, typeof(FolderController).Name);
            _logger.Log(logMessage);
        }
    }
}