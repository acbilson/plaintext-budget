using PTB.Core;
using PTB.Core.FolderAccess;
using PTB.Core.Logging;
using PTB.Reports.Budget;
using System;
using System.IO;

namespace PTB.Reports.FolderAccess
{
    public class BudgetFolderService : BaseFolderService
    {
        public BudgetFolderService(PTBSettings settings, BudgetSchema schema, IPTBLogger logger) : base(settings, schema, logger)
        {
        }

        public PTBFolder<CategoriesFile> GetFolder()
        {
            return base.GetFolder<CategoriesFile>();
        }

        private string GetNewBudgetFileName()
        {
            string startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yy-MM-dd");
            string endDay = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
            if (endDay.Length == 1) { endDay = string.Concat("0", endDay); }
            string fileName = $"budget{_settings.FileDelimiter}{startDate}_to_{endDay}{_settings.FileExtension}";
            return fileName;

        }

        public BudgetFile CreateNewBudgetFile()
        {
            string fileName = GetNewBudgetFileName();
            string filePath = Path.Combine(_settings.HomeDirectory, _schema.Folder, fileName);
            File.Create(filePath).Dispose();
            return new BudgetFile(_settings.FileDelimiter, _schema.LineSize, new FileInfo(filePath));
        }
    }
}