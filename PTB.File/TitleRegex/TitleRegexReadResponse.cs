using System.Collections.Generic;

namespace PTB.File.TitleRegex
{
    public class CategoriesReadResponse
    {
        public List<TitleRegex> TitleRegices;

        public List<string> SkippedMessages;

        public static CategoriesReadResponse Default => new CategoriesReadResponse { TitleRegices = new List<TitleRegex>(), SkippedMessages = new List<string>() };
    }
}