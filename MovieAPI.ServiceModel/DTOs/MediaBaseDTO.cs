using System.Text.Json.Serialization;

namespace MovieAPI.ServiceModel.DTOs
{
    [JsonDerivedType(typeof(MediaBaseDTO), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(MovieDTO), typeDiscriminator: "withMovie")]
    [JsonDerivedType(typeof(TvSerieDTO), typeDiscriminator: "withSerie")]
    public class MediaBaseDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("adult")]
        public bool IsAdult { get; set; }

        [JsonPropertyName("backdrop_path")]
        public string? BackdropPath { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }

        [JsonPropertyName("genre_ids")]
        public List<int> GenreIds { get; set; } = new List<int>();

        [JsonPropertyName("original_language")]
        public string? OriginalLanguage { get; set; }

        [JsonPropertyName("overview")]
        public string? Overview { get; set; }
    }
}
