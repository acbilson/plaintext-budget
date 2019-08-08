using System;
using System.Collections.Generic;
using System.Text;

namespace PTB.Core
{
    public struct Constant
    {
        public const short TRANSACTION_SIZE = 115;
        public const string NOISE_CHARS = "'\"\\-\\*\\.\\#\\`\\\\\\/ ";
    }

    public struct ColumnSize
    {
        public const short DATE = 10;
        public const short TYPE = 1;
        public const short AMOUNT = 12;
        public const short SUBCATEGORY = 20;
        public const short TITLE = 50;
        public const short LOCATION = 15;
        public const short LOCKED = 1;
    }
    public struct ColumnIndex
    {
        public static readonly short[] DATE = new short[2] { 0, ColumnSize.DATE };
        public static readonly short[] TYPE = new short[2] { 10 + 1, ColumnSize.TYPE };
        public static readonly short[] AMOUNT = new short[2] { 11 + 2, ColumnSize.AMOUNT };
        public static readonly short[] SUBCATEGORY = new short[2] { 23 + 3, ColumnSize.SUBCATEGORY };
        public static readonly short[] TITLE = new short[2] { 43 + 4, ColumnSize.TITLE };
        public static readonly short[] LOCATION = new short[2] { 93 + 5, ColumnSize.LOCATION };
        public static readonly short[] LOCKED = new short[2] { 108 + 6, ColumnSize.LOCKED };
    }
}
