using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.Parsers
{
    public interface IStatementParser
    {
        string ParseLine(string line);
    }
}
