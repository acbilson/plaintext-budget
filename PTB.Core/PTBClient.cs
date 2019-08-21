using Newtonsoft.Json;
using PTB.Core.Categories;
using PTB.Core.Budget;
using PTB.Core.Ledger;
using PTB.Core.TitleRegex;
using PTB.Core.Logging;

namespace PTB.Core
{
    public class PTBClient
    {
        private static readonly PTBClient _instance = new PTBClient();
        public static PTBClient Instance => _instance;

        public FileManager FileManager;

        public LedgerRepository Ledger;
        public TitleRegexRepository Regex;
        public BudgetRepository Budget;
        public CategoriesRepository Categories;

        private PTBClient() { }

        public void Instantiate(FileManager fileManager, PTBFileLogger logger)
        {
            FileManager = fileManager;
            Ledger = new LedgerRepository(fileManager);
            Regex = new TitleRegexRepository(fileManager);
            Budget = new BudgetRepository(fileManager);
            Categories = new CategoriesRepository(fileManager, logger);
        }
    }
}