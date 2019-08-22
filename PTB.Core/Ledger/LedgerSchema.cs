using PTB.Core.Base;
using System.Linq;

namespace PTB.Core.Ledger
{
    public class LedgerSchema : Base.FolderSchema
    {
        public string FileMask { get; set; }
    }
}