using PTB.File.Ledger;

namespace PTB.File.Statements
{
    public interface IStatementParser
    {
        ParseResponse ParseLine(string line, LedgerSchema schema);
    }
}