namespace PTB.File.Categories
{
    public class CategoriesToStringResponse
    {
        public string Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static CategoriesToStringResponse Default => new CategoriesToStringResponse { Result = string.Empty, Success = true, Message = string.Empty };
    }

    public class StringToCategoriesResponse
    {
        public Categories Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static StringToCategoriesResponse Default => new StringToCategoriesResponse { Success = true, Message = string.Empty };
    }
}