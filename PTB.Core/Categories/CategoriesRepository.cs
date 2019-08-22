using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using PTB.Core.Exceptions;
using PTB.Core.Logging;
using PTB.Core.Base;

namespace PTB.Core.Categories
{
    public class CategoriesRepository : BaseFileRepository
    {
        public CategoriesRepository(IPTBLogger logger, BaseParser parser, FolderSchema schema, BasePTBFile file) : base(logger, parser, schema, file)
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