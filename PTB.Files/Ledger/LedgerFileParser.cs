using PTB.Core.Files;
using PTB.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Files.Ledger
{
    public class LedgerFileParser : BaseFileParser
    {
        public LedgerFileParser(LedgerSchema schema, IPTBLogger logger): base(schema, logger)
        {

        }
    }
}
