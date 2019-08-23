using PTB.Core.Base;
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
        protected PTBFile _file;

        public BaseReportRepository(IPTBLogger logger, BaseReportParser parser, FolderSchema schema, PTBFile file)
        {
            _logger = logger;
            _schema = schema;
            _parser = parser;
            _file = file;
        }

        public BaseAppendResponse Append(PTBRow row)
        {
            throw new NotImplementedException();
        }

        public BaseReadResponse Read(int index, int count)
        {
            throw new NotImplementedException();
        }

        public BaseUpdateResponse Update(int index, PTBRow row)
        {
            throw new NotImplementedException();
        }
    }
}