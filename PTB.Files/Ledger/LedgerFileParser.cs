using PTB.Core.Files;
using PTB.Core.Logging;

namespace PTB.Files.Ledger
{
    public class LedgerFileParser : BaseFileParser
    {
        public LedgerFileParser(LedgerSchema schema, IPTBLogger logger) : base(schema, logger)
        {
        }
    }
}