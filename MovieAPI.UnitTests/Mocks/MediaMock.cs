using MovieAPI.Core.Models;

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
                ReleaseDate = "2021-04-01",
                Overview = "Overview 1",
                PosterPath = "PosterPath 1"
            },
            new Movie
            {
                Id = 2,
                OriginalTitle = "Movie 2",
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
                FirstAirDate = "2021-04-02",
                Overview = "Overview 1",
                PosterPath = "PosterPath 1"
            },
            new TvSerie
            {
                Id = 2,
                Name = "TvSerie 2",
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
}
