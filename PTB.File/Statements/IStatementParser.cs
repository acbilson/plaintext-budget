using PTB.File.Ledger;

namespace PTB.File.Statements
{
    public interface IStatementParser
    {
        StatementParseResponse ParseLine(string line, LedgerSchema schema);
    }
}