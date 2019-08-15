using System.Collections.Generic;

namespace PTB.File.TitleRegex
{
    public class TitleRegexReadResponse
    {
        public List<TitleRegex> TitleRegices;

        public List<string> SkippedMessages;

        public static TitleRegexReadResponse Default => new TitleRegexReadResponse { TitleRegices = new List<TitleRegex>(), SkippedMessages = new List<string>() };
    }
}