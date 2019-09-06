using PTB.Core.Base;
using PTB.Core.Files;
using PTB.Core.Logging;
using PTB.Core.Reports;
using PTB.Reports.FolderAccess;

namespace PTB.Reports.Categories
{
    public class CategoriesService : BaseReportService
    {
        public CategoriesService(IPTBLogger logger, CategoriesFileParser parser, CategoriesSchema schema) : base(logger, parser, schema)
        {
            _logger.SetContext(nameof(CategoriesService));
        }

        public BaseReadResponse Read(CategoriesFile file)
        {
            var response = BaseReadResponse.Default;

            int startIndex = 0;

            response = base.Read(file, startIndex, file.LineCount);

            return response;
        }
    }
}