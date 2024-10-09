using MovieAPI.Core.Attributes;

namespace MovieAPI.Core.Helpers;

public static class MediaTypeHelper
{
    /// <summary>
    /// Determines the media type URL for a given type using reflection.
    /// </summary>
    /// <returns>The media type URL or an empty string if not found.</returns>
    public static string DetermineMediaTypeUrl<TMedia>()
    {
        var typeMedia = typeof(TMedia);
        var attribute = Attribute.GetCustomAttribute(typeMedia, typeof(MediaTypeUrlAttribute)) as MediaTypeUrlAttribute;
        return attribute?.MediaTypeUrl ?? string.Empty;
    }
}
