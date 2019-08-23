using PTB.Core.Base;
using PTB.Core.Files;
using PTB.Core.Logging;
using PTB.Files.FolderAccess;

namespace PTB.Files.TitleRegex
{
    public class TitleRegexRepository : BaseFileService
    {
        public TitleRegexRepository(IPTBLogger logger, BaseFileParser parser, FolderSchema schema) : base(logger, parser, schema)
        {
            _logger.SetContext(nameof(TitleRegexRepository));
        }

        public BaseReadResponse ReadAllTitleRegex(TitleRegexFile file)
        {
            var response = BaseReadResponse.Default;

            response = base.Read(file, 0, file.LineCount);

            return response;
        }
    }
}