using System;
using System.Collections.Generic;
using System.Text;
using PTB.File.Ledger;
using PTB.File.TitleRegex;

namespace PTB.File
{
    public class PTBSchema
    {
        public LedgerSchema Ledger;
        public TitleRegexSchema Regex;
    }
}
