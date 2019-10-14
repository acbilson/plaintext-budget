using PTB.Core.Base;
using PTB.Core.Files;
using PTB.Core.Logging;

namespace PTB.Reports.Categories
{
    public class CategoriesFileParser : BaseReportParser
    {
        public CategoriesFileParser(CategoriesSchema schema, IPTBLogger logger) : base(schema, logger)
        {
        }
    }
}