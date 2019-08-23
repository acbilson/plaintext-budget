using PTB.Core.Base;
using System.Collections.Generic;

namespace PTB.Files.TitleRegex
{
    public class TitleRegexReadResponse : BaseReadResponse
    {
        public List<string> SkippedMessages;

        public new static TitleRegexReadResponse Default => new TitleRegexReadResponse { Success = true, Message = string.Empty, ReadResult = new List<PTBRow>(), SkippedMessages = new List<string>() };
    }
}