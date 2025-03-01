﻿using MovieAPI.Core.Attributes;

namespace MovieAPI.Core.Models
{
    [MediaTypeUrl("movie?")]
    public class Movie : MediaBase
    {
        public string OriginalTitle { get; set; } = string.Empty;
        public string? ReleaseDate { get; set; }
        public override string Title => OriginalTitle;
    }
}
