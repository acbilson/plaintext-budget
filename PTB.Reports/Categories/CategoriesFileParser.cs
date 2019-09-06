﻿using PTB.Core.Files;
using PTB.Core.Logging;

namespace PTB.Reports.Categories
{
    public class CategoriesFileParser : BaseFileParser
    {
        public CategoriesFileParser(CategoriesSchema schema, IPTBLogger logger) : base(schema, logger)
        {
        }
    }
}