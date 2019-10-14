using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PTB.Core.Base;
using PTB.Core.Exceptions;
using PTB.Core.Logging;
using PTB.Reports.Budget;
using PTB.Reports.FolderAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        // GET: api/Budget/Read?start=2019-01-01&end=2019-01-31
        [HttpGet("[action]")]
        public List<PTBRow> Read(string start, string end)
        {
            var fileFolders = _reportFolderService.GetFolders();

            DateTime startDate = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var budgetFile = fileFolders.BudgetFolder.Files.First(file => file.StartDate == startDate && file.EndDate == endDate);
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

        // PUT: api/Budget/Update
        [HttpPut("[action]")]
        public PTBRow Update([FromBody] PTBRow record)
        {
            var reportFolders = _reportFolderService.GetFolders();
            var defaultBudgetFile = reportFolders.BudgetFolder.GetDefaultFile();
            var response = _budgetService.Update(defaultBudgetFile, record.Index, record);

            if (!response.Success)
            {
                string message = $"Failed to update budget record {defaultBudgetFile.ShortName} at index: {record.Index}. Message was {response.Message}";
                LogError(message);
                throw new WebException(message);
            }

            Log($"Updated budget record: {Environment.NewLine} {record.ToString()}");
            return record;
        }

    }
}