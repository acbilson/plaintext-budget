using PTB.Core.Base;
using PTB.Core.Ledger;

namespace PTB.Core.Statements
{
    public interface IStatementParser
    {
        StatementParseResponse ParseLine(string line);
    }
}