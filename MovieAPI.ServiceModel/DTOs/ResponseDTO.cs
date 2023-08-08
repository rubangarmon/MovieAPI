using System.Text.Json.Serialization;

namespace MovieAPI.ServiceModel.DTOs
{
    public class ResponseDTO<T>
    {
        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; } = 0;

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; } = 0;

        [JsonPropertyName("results")]
        public IEnumerable<T> Movies { get; set; } = new List<T>();
    }
}
