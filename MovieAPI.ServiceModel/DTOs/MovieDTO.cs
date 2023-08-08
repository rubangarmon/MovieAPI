using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieAPI.ServiceModel.DTOs
{
    public class MovieDTO : MediaBaseDTO
    {
        [JsonPropertyName("original_title")]
        public string? OriginalTitle { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        //[JsonPropertyName("video")]
        //public string? Video { get; set; }
    }
}
