using MovieAPI.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MovieAPI.Core.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Movie), "movie")]
[JsonDerivedType(typeof(TvSerie), "tvSerie")]
[MediaTypeUrl("multi?")]
public abstract class MediaBase
{
    public int Id { get; set; }
    public abstract string Title { get;  }
    public bool IsAdult { get; set; }
    public string? PosterPath { get; set; }
    public string Overview { get; set; }

}
