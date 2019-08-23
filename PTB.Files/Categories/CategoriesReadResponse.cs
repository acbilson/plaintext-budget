using PTB.Core.Base;
using System.Collections.Generic;

namespace PTB.Files.Categories
{
    public class CategoriesReadResponse : BaseReadResponse
    {
        public List<string> SkippedMessages;

        public new static CategoriesReadResponse Default => new CategoriesReadResponse { Success = true, Message = string.Empty, SkippedMessages = new List<string>(), ReadResult = new List<PTBRow>() };
    }
}