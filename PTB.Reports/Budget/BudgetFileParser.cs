using PTB.Core.Base;
using PTB.Core.Logging;

namespace PTB.Reports.Budget
{
    public class BudgetFileParser : BaseReportParser
    {
        public BudgetFileParser(BudgetSchema schema, IPTBLogger logger) : base(schema, logger)
        {
        }
    }
}