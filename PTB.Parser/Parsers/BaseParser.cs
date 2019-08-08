using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core.Parsers
{
    public abstract class BaseParser
    {
        public abstract Transaction Parse(string line);
    }
}
