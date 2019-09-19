using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PTB.Core.Logging;
using PTB.Files;
using PTB.Files.FolderAccess;
using System;
using System.IO;

namespace PTB.Web.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : BasePTBController
    {
        private FileFolderService _fileFolderService;

        public FolderController(FileFolderService fileFolderService, IPTBLogger logger) : base(logger)
        {
            _fileFolderService = fileFolderService;
            logger.SetContext(nameof(FolderController));
        }

        // GET: api/Folder/GetFileFolders
        [HttpGet("[action]")]
        public FileFolders GetFileFolders()
        {
            FileFolders folders = new FileFolders();

            try
            {
                folders = _fileFolderService.GetFolders();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }

            return folders;
        }

        // GET: api/Folder/GetFileSchema
        [HttpGet("[action]")]
        public FileSchema GetFileSchema()
        {
            string home = Environment.GetEnvironmentVariable("ONEDRIVECOMMERCIAL");
            string baseDir = Path.Combine(home, @"Archive\2019\BudgetProject\PTB_Home");

            var schemaText = System.IO.File.ReadAllText(Path.Combine(baseDir, "schema.json"));
            var schema = JsonConvert.DeserializeObject<FileSchema>(schemaText);

            return schema;
        }
    }
}