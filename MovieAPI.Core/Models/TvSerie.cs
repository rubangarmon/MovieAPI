using MovieAPI.Core.Attributes;

namespace MovieAPI.Core.Models
{
    [MediaTypeUrl("tv?")]
    public class TvSerie : MediaBase
    {
        public override string Title => OriginalName;
        public string? OriginalName { get; set; }

        public string? FirstAirDate { get; set; }

        public string? Name { get; set; }
    }
}
