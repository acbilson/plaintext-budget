using PTB.Core.Base;
using PTB.Core.Files;
using PTB.Core.Logging;

namespace PTB.Files.Categories
{
    public class CategoriesRepository : BaseFileRepository
    {
        public CategoriesRepository(IPTBLogger logger, BaseFileParser parser, FolderSchema schema, PTBFile file) : base(logger, parser, schema, file)
        {
            _logger.SetContext(nameof(CategoriesRepository));
        }

        public BaseReadResponse ReadAllDefaultCategories()
        {
            var response = BaseReadResponse.Default;

            int startIndex = 0;

            response = base.Read(startIndex, _file.LineCount);

            return response;
        }
    }
}