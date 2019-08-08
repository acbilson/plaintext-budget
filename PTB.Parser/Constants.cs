using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core
{
    public struct Constant
    {
        public const int TRANSACTION_SIZE = 115;
        public const string NOISE_CHARS = "'\"\\-\\*\\.\\#\\`\\\\\\/ ";
    }

    public struct ColumnSize
    {
        public const int DATE = 10;
        public const int AMOUNT = 12;
        public const int SUBCATEGORY = 20;
        public const int TYPE = 1;
        public const int LOCKED = 1;
        public const int TITLE = 50;
        public const int LOCATION = 15;
    }
}
