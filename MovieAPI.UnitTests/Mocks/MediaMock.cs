using MovieAPI.Core.Models;
using MovieAPI.ServiceModel.DTOs;

namespace MovieAPI.UnitTests.Mocks;

public static class MediaMock
{
    public static List<Movie> GetMoviesMocks()
    {
        return
        [
            new Movie
            {
                Id = 1,
                OriginalTitle = "Movie 1",
                IsAdult = true,
                ReleaseDate = "2021-04-01",
                Overview = "Overview 1",
                PosterPath = "PosterPath 1"
            },
            new Movie
            {
                Id = 2,
                OriginalTitle = "Movie 2",
                IsAdult = false,
                ReleaseDate = "2021-04-02",
                Overview = "Overview 2",
                PosterPath = "PosterPath 2"
            }
        ];
    }
    public static List<TvSerie> GetTvSeriesMocks()
    {
        return
        [
            new TvSerie
            {
                Id = 1,
                Name = "TvSerie 1",
                IsAdult = false,
                OriginalName = "TvSerie Original Name 1",
                FirstAirDate = "2021-04-02",
                Overview = "Overview 1",
                PosterPath = "PosterPath 1"
            },
            new TvSerie
            {
                Id = 2,
                Name = "TvSerie 2",
                IsAdult = false,
                OriginalName = "TvSerie Original Name 2",
                FirstAirDate = "2021-04-02",
                Overview = "Overview 2",
                PosterPath = "PosterPath 2"
            }
        ];
    }
    public static List<MediaBase> GetMultiMocks()
    {
        var series = GetTvSeriesMocks();
        var movies = GetMoviesMocks();
        return series.Cast<MediaBase>().Concat(movies.Cast<MediaBase>()).ToList();
    }

    public static List<MovieDTO> ToDTO(this List<Movie> movies)
    {
        return movies.Select(m => new MovieDTO
        {
            Id = m.Id,
            OriginalTitle = m.OriginalTitle,
            IsAdult = m.IsAdult,
            ReleaseDate = m.ReleaseDate,
            Overview = m.Overview,
            PosterPath = m.PosterPath
        }).ToList();
    }

    public static List<TvSerieDTO> ToDTO(this List<TvSerie> tvSeries)
    {
        return tvSeries.Select(m => new TvSerieDTO
        {
            Id = m.Id,
            Name = m.Name,
            IsAdult = m.IsAdult,
            OriginalName = m.OriginalName,
            FirstAirDate = m.FirstAirDate,
            Overview = m.Overview,
            PosterPath = m.PosterPath
        }).ToList();
    }
}
