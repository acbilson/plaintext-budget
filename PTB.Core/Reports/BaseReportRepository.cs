using PTB.Core.Base;
using PTB.Core.FolderAccess;
using PTB.Core.Files;
using PTB.Core.Logging;
using System;

namespace PTB.Core.Reports
{
    public class BaseReportRepository : IPTBRepository
    {
        protected IPTBLogger _logger;
        protected BaseReportParser _parser;
        protected FolderSchema _schema;

        public BaseReportRepository(IPTBLogger logger, BaseReportParser parser, FolderSchema schema)
        {
            _logger = logger;
            _schema = schema;
            _parser = parser;
        }

        public BaseAppendResponse Append(BasePTBFile file, PTBRow row)
        {
            throw new NotImplementedException();
        }

        public BaseReadResponse Read(BasePTBFile file, int index, int count)
        {
            throw new NotImplementedException();
        }

        public BaseUpdateResponse Update(BasePTBFile file, int index, PTBRow row)
        {
            throw new NotImplementedException();
        }
    }
}