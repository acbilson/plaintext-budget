using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.File.Ledger
{
    public class LedgerUpdateResponse
    {
        public bool Success;
        public string Message;

        public static LedgerUpdateResponse Default => new LedgerUpdateResponse { Success = true, Message = string.Empty };
    }
}
