namespace MovieAPI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MediaTypeUrlAttribute(string mediaTypeUrl) : Attribute
    {
        public string MediaTypeUrl { get; } = mediaTypeUrl;
    }
}
