using System.Collections.Generic;

namespace PTB.Core.TitleRegex
{
    public class TitleRegexToStringResponse
    {
        public string Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static TitleRegexToStringResponse Default => new TitleRegexToStringResponse { Result = string.Empty, Success = true, Message = string.Empty };
    }

    public class StringToTitleRegexResponse
    {
        public TitleRegex Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static StringToTitleRegexResponse Default => new StringToTitleRegexResponse { Success = true, Message = string.Empty };
    }

    public class TitleRegexReadResponse
    {
        public List<TitleRegex> TitleRegices;

        public List<string> SkippedMessages;

        public static TitleRegexReadResponse Default => new TitleRegexReadResponse { TitleRegices = new List<TitleRegex>(), SkippedMessages = new List<string>() };
    }
}