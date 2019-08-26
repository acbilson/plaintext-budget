using PTB.Core.Files;
using PTB.Core.Logging;

namespace PTB.Files.TitleRegex
{
    public class TitleRegexFileParser : BaseFileParser
    {
        public TitleRegexFileParser(TitleRegexSchema schema, IPTBLogger logger) : base(schema, logger)
        {
        }
    }
}