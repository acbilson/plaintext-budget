using PTB.File.Ledger;

namespace PTB.File.Statements
{
    public interface IStatementParser
    {
        string ParseLine(string line, LedgerSchema schema);
    }
}