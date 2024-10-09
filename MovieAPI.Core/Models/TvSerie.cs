using MovieAPI.Core.Attributes;

namespace MovieAPI.Core.Models
{
    [MediaTypeUrl("serie?")]
    public class TvSerie : MediaBase
    {
        public string? OriginalName { get; set; }

        public string? FirstAirDate { get; set; }

        public string? Name { get; set; }
    }
}
