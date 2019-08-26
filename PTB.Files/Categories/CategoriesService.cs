using PTB.Core.Base;
using PTB.Core.Files;
using PTB.Core.Logging;
using PTB.Files.FolderAccess;

namespace PTB.Files.Categories
{
    public class CategoriesService : BaseFileService
    {
        public CategoriesService(IPTBLogger logger, CategoriesFileParser parser, FolderSchema schema) : base(logger, parser, schema)
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