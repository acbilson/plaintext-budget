using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.File.Exceptions
{
    public class ParseException : Exception
    {
        public ParseException() { }

        public ParseException(string message) : base(message) { }

        public ParseException(string message, Exception inner) : base(message, inner) { }
    }
}
