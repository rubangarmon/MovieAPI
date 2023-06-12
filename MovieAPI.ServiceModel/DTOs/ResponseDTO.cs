using Newtonsoft.Json;

namespace MovieAPI.ServiceModel.DTOs
{
    public class ResponseDTO
    {
        [JsonProperty("page")]
        public int Page { get; set; } = 1;

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; } = 0;

        [JsonProperty("total_results")]
        public int TotalResults { get; set; } = 0;

        [JsonProperty("results")]
        public IEnumerable<MovieDTO> Movies { get; set; } = new List<MovieDTO>();
    }
}
