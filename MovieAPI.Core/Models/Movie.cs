using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Core.Models
{
    public class Movie : IMovie
    {
        public bool IsAdult { get; set; }
        public string? BackdropPath { get; set; }
        public List<int> GenreIds { get; set; } = new List<int>();
        public int Id { get; set; }
        public string OriginalLanguage { get; set; } = "en";
        public string OriginalTitle { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public double Popularity { get; set; }
        public string? PosterPath { get; set; }
        public string? ReleaseDate { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool Video { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
    }
}
