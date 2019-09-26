using PTB.Core.Base;
using PTB.Core.Files;
using PTB.Core.Logging;
using PTB.Files.FolderAccess;

namespace PTB.Files.TitleRegex
{
    public class TitleRegexService : BaseFileService
    {
        public TitleRegexService(IPTBLogger logger, TitleRegexFileParser parser, TitleRegexSchema schema, FileValidation validator) : base(logger, parser, schema, validator)
        {
            _logger.SetContext(nameof(TitleRegexService));
        }

        public BaseReadResponse Read(TitleRegexFile file)
        {
            var response = BaseReadResponse.Default;

            response = base.Read(file, 0, file.LineCount);

            return response;
        }
    }
}