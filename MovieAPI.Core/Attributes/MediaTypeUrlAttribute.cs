namespace MovieAPI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MediaTypeUrlAttribute : Attribute
    {
        public string MediaTypeUrl { get; }

        public MediaTypeUrlAttribute(string mediaTypeUrl)
        {
            MediaTypeUrl = mediaTypeUrl;
        }
    }
}
