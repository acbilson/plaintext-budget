using PTB.Core.Base;

namespace PTB.Core.Statements
{
    public interface IStatementParser
    {
        StatementParseResponse ParseLine(string line);
    }
}