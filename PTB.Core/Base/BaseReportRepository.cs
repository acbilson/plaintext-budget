using PTB.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.Base
{
    public class BaseReportRepository : IPTBRepository
    {
        protected IPTBLogger _logger;
        protected BaseParser _parser;
        protected FolderSchema _schema;
        protected BasePTBFile _file;

        public BaseReportRepository(IPTBLogger logger, BaseParser parser, FolderSchema schema, BasePTBFile file)
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
