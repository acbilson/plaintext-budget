namespace PTB.Core
{
    public struct Constant
    {
        public const string NOISE_CHARS = "'\"\\-\\*\\.\\#\\`\\\\\\/ ";
    }

    public struct ParseMessages
    {
        public const string EMPTY_LINE = "Empty Line";
        public const string LINE_LENGTH_MISMATCH_SCHEMA = "Line length does not match schema, which may indicate data corruption.";
        public const string LINE_NO_CR = "Line does not end with carriage return, which may indicate data corruption";
    }
}