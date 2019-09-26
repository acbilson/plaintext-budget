namespace PTB.Core
{
    public struct Constant
    {
        public const string NOISE_CHARS = "'\"\\-\\*\\.\\#\\`\\\\\\/ ";
    }

    public struct ParseMessages
    {
        public const string EMPTY_LINE = "Empty Line";
        public const string LINE_LENGTH_MISMATCH_SCHEMA = "Line length does not match schema";
        public const string LINE_NO_CR = "Line does not end with carriage return";
        public const string LINE_BOM = "The {0} ledger has a byte order mark added by a utf-8 compatible editor. Please remove from the text file to continue.";
        public const string LINE_UNIX_NEWLINE = "The {0} ledger has Unix new lines instead of Windows new lines. Please convert to Windows new lines to continue.";
        public const string LINE_START_INDEX = "The start index {0} to update file {1} does not match the index of any line. It should be divisible by {2}";
        public const string LINE_COLUMN_MISMATCH = "The row at index {0} has the following column size mismatches: {1}{2}";
        public const string LINE_DATA_CORRUPTION = "Review file {0} for data corruption at line {1}. Message is: {2}";
        public const string LINE_INDEX_MISSING = "The start index {0} to update file {1} does not match the index of any line. It should be divisible by {2}";
    }
}