namespace MovieAPI.Application.Configuration
{
    public class HttpMovieRepositoryOptions
    {
        public static string OptionName = "HttpMovieRepository";
        public string BaseURL { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
    }
}
