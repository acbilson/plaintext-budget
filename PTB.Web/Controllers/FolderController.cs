using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PTB.Core.Logging;
using PTB.Files.FolderAccess;
using System;

namespace PTB.Web.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
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

        // GET: api/Folder/GetFileFolders
        [HttpGet("[action]")]
        public FileFolders GetFileFolders()
        {
            FileFolders folders = new FileFolders();

            try
            {
                folders = _fileFolderService.GetFileFolders();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }

            return folders;
        }

        private void Log(string message)
        {
            var logMessage = new LogMessage(LoggingLevel.Debug, message, typeof(FolderController).Name);
            _logger.Log(logMessage);
        }

        private void LogError(string message)
        {
            var logMessage = new LogMessage(LoggingLevel.Error, message, typeof(FolderController).Name);
            _logger.Log(logMessage);
        }
    }
}