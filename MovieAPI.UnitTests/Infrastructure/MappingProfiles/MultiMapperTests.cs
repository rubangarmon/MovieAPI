using MovieAPI.Core.Models;
using MovieAPI.Infrastructure.MappingProfiles;
using MovieAPI.ServiceModel.DTOs;
using Shouldly;

namespace MovieAPI.UnitTests.Infrastructure.MappingProfiles;

public class MultiMapperTests
{
    [Fact]
    public void MapMultiToMediaBase_ShouldMapMovieDTOToMovie()
    {
        // Arrange
        var responseDTO = new ResponseDTO<MultiDTO>
        {
            Page = 1,
            TotalPages = 1,
            TotalResults = 1,
            Results = new List<MultiDTO>
            {
                new MultiDTO
                {
                    Id = 1,
                    MediaType = "movie",
                    Title = "Test Movie",
                    Overview = "Test Overview",
                    PosterPath = "/test.jpg",
                    ReleaseDate = "2023-01-01",
                    IsAdult = false
                }
            }
        };

        // Act
        var result = MultiMapper.MapMultiToMediaBase(responseDTO);

        // Assert
        result.ShouldNotBeNull();
        result.Page.ShouldBe(responseDTO.Page);
        result.TotalPages.ShouldBe(responseDTO.TotalPages);
        result.TotalResults.ShouldBe(responseDTO.TotalResults);
        result.Results.ShouldHaveSingleItem();
        var movie = result.Results.ShouldBeOfType<List<MediaBase>>().First() as Movie;
        movie.ShouldNotBeNull();
        movie.Id.ShouldBe(1);
        movie.OriginalTitle.ShouldBe("Test Movie");
        movie.Overview.ShouldBe("Test Overview");
        movie.PosterPath.ShouldBe("/test.jpg");
        movie.ReleaseDate.ShouldBe("2023-01-01");
        movie.IsAdult.ShouldBeFalse();
    }

    [Fact]
    public void MapMultiToMediaBase_ShouldMapTvSerieDTOToTvSerie()
    {
        // Arrange
        var responseDTO = new ResponseDTO<MultiDTO>
        {
            Page = 1,
            TotalPages = 1,
            TotalResults = 1,
            Results = new List<MultiDTO>
            {
                new MultiDTO
                {
                    Id = 1,
                    MediaType = "tv",
                    Name = "Test TV Series",
                    OriginalName = "Test Original Name",
                    Overview = "Test Overview",
                    PosterPath = "/test.jpg",
                    FirstAirDate = "2023-01-01",
                    IsAdult = false
                }
            }
        };

        // Act
        var result = MultiMapper.MapMultiToMediaBase(responseDTO);

        // Assert
        result.ShouldNotBeNull();
        result.Page.ShouldBe(responseDTO.Page);
        result.TotalPages.ShouldBe(responseDTO.TotalPages);
        result.TotalResults.ShouldBe(responseDTO.TotalResults);
        result.Results.ShouldHaveSingleItem();
        var tvSerie = result.Results.ShouldBeOfType<List<MediaBase>>().First() as TvSerie;
        tvSerie.ShouldNotBeNull();
        tvSerie.Id.ShouldBe(1);
        tvSerie.Name.ShouldBe("Test TV Series");
        tvSerie.OriginalName.ShouldBe("Test Original Name");
        tvSerie.Overview.ShouldBe("Test Overview");
        tvSerie.PosterPath.ShouldBe("/test.jpg");
        tvSerie.FirstAirDate.ShouldBe("2023-01-01");
        tvSerie.IsAdult.ShouldBeFalse();
    }

    [Fact]
    public void MapMultiToMediaBase_ShouldThrowArgumentExceptionForUnknownMediaType()
    {
        // Arrange
        var responseDTO = new ResponseDTO<MultiDTO>
        {
            Page = 1,
            TotalPages = 1,
            TotalResults = 1,
            Results = new List<MultiDTO>
            {
                new MultiDTO
                {
                    Id = 1,
                    MediaType = "unknown",
                    Title = "Test Unknown",
                    Overview = "Test Overview",
                    PosterPath = "/test.jpg",
                    IsAdult = false
                }
            }
        };

        // Act & Assert
        Should.Throw<ArgumentException>(() => MultiMapper.MapMultiToMediaBase(responseDTO));
    }

    [Fact]
    public void MapMultiToMediaBase_ShouldHandleEmptyResults()
    {
        // Arrange
        var responseDTO = new ResponseDTO<MultiDTO>
        {
            Page = 1,
            TotalPages = 1,
            TotalResults = 0,
            Results = new List<MultiDTO>()
        };

        // Act
        var result = MultiMapper.MapMultiToMediaBase(responseDTO);

        // Assert
        result.ShouldNotBeNull();
        result.Page.ShouldBe(responseDTO.Page);
        result.TotalPages.ShouldBe(responseDTO.TotalPages);
        result.TotalResults.ShouldBe(responseDTO.TotalResults);
        result.Results.ShouldBeEmpty();
    }
}
