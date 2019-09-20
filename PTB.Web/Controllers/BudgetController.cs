using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PTB.Core.Base;
using PTB.Core.Exceptions;
using PTB.Core.Logging;
using PTB.Reports.Budget;
using PTB.Reports.FolderAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PTB.Web.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : BasePTBController
    {
        private ReportFolderService _reportFolderService;
        private BudgetService _budgetService;

        public BudgetController(ReportFolderService reportFolderService, BudgetService budgetService, IPTBLogger logger) : base(logger)
        {
            _reportFolderService = reportFolderService;
            _budgetService = budgetService;
            logger.SetContext(nameof(BudgetController));
        }

        // GET: api/Budget/Read?fileName=checking&startIndex=0&count=10
        [HttpGet("[action]")]
        public List<PTBRow> Read(string fileName)
        {
            var fileFolders = _reportFolderService.GetFolders();
            var budgetFile = fileFolders.BudgetFolder.Files.First(file => file.ShortName == fileName);
            var response = _budgetService.Read(budgetFile, 0, budgetFile.LineCount);

            if (!response.Success)
            {
                string message = $"Failed to read budget {budgetFile.ShortName}. Message was: {response.Message}";
                LogError(message);
                throw new WebException(message);
            }

            Log($"Read {response.ReadResult.Count} lines from budget {budgetFile.ShortName}");
            int randomIndex = new Random().Next(0, response.ReadResult.Count - 1);
            Log($"Sample budget: {response.ReadResult[randomIndex]}");
            return response.ReadResult;
        }
    }
}