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
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private IPTBLogger _logger;
        private PTBSettings _settings;
        private PTBSchema _schema;

        public FolderController(PTBSettings settings, PTBSchema schema, IPTBLogger logger)
        {
            _logger = logger;
        }

        // GET: api/File/GetFileFolders
        [HttpGet("[action]")]
        public FileFolders GetFileFolders()
        {
            var client = new PTBClient(" ");
            var folderMgr = new FileFolderManager(_settings, _schema.Ledger, _logger);
            var folders = folderMgr.GetFileFolders();
            return folders;
        }

        private void Log(string message)
        {
            var logMessage = new LogMessage(LoggingLevel.Debug, message, typeof(FolderController).Name);
            _logger.Log(logMessage);
        }
    }
}