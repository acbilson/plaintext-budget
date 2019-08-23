using PTB.Core.Base;
using PTB.Core.Files;
using PTB.Core.Logging;

namespace PTB.Files.TitleRegex
{
    public class TitleRegexRepository : BaseFileRepository
    {
        public TitleRegexRepository(IPTBLogger logger, BaseFileParser parser, FolderSchema schema, PTBFile file) : base(logger, parser, schema, file)
        {
            _logger.SetContext(nameof(TitleRegexRepository));
        }

        public BaseReadResponse ReadAllTitleRegex()
        {
            var response = BaseReadResponse.Default;

            response = base.Read(0, _file.LineCount);

            return response;
        }
    }
}