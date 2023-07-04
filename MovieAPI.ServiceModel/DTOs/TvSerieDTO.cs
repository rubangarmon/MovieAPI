
using System.Text.Json.Serialization;

namespace MovieAPI.ServiceModel.DTOs
{
    public class TvSerieDTO : MediaBaseDTO
    {
        [JsonPropertyName("original_name")]
        public string? OriginalName { get; set; }

        [JsonPropertyName("first_air_date")]
        public string? FirstAirDate{ get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
