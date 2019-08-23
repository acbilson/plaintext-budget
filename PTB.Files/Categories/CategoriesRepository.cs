using PTB.Core.Base;
using PTB.Core.FolderAccess;
using PTB.Core.Files;
using PTB.Core.Logging;
using PTB.Files.FolderAccess;

namespace PTB.Files.Categories
{
    public class CategoriesRepository : BaseFileService
    {
        public CategoriesRepository(IPTBLogger logger, BaseFileParser parser, FolderSchema schema) : base(logger, parser, schema)
        {
            _logger.SetContext(nameof(CategoriesRepository));
        }

        public BaseReadResponse ReadAllDefaultCategories(CategoriesFile file)
        {
            var response = BaseReadResponse.Default;

            int startIndex = 0;

            response = base.Read(file, startIndex, file.LineCount);

            return response;
        }
    }
}