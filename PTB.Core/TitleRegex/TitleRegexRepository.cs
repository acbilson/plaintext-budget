using PTB.Core.Base;
using PTB.Core.Exceptions;
using PTB.Core.Logging;
using System;
using System.IO;
using System.Text;

namespace PTB.Core.TitleRegex
{
    public class TitleRegexRepository : BaseFileRepository
    {
        public TitleRegexRepository(IPTBLogger logger, BaseParser parser, FolderSchema schema, BasePTBFile file) : base(logger, parser, schema, file)
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