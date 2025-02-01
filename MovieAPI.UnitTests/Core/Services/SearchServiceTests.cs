using Moq;
using MovieAPI.Core.HttpClients;
using MovieAPI.Core.Models;
using MovieAPI.Core.Services;
using MovieAPI.UnitTests.Mocks;
using Shouldly;

namespace MovieAPI.UnitTests.Core.Services
{
    public class SearchServiceTests
    {
        private readonly Mock<IHttpMediaRepository<Movie>> _mockMovieRepository;
        private readonly Mock<IHttpMediaRepository<TvSerie>> _mockTvRepository;
        private readonly Mock<IHttpMediaRepository<MediaBase>> _mockMultiRepository;
        private readonly SearchService _searchService;

        public SearchServiceTests()
        {
            _mockMovieRepository = new Mock<IHttpMediaRepository<Movie>>();
            _mockTvRepository = new Mock<IHttpMediaRepository<TvSerie>>();
            _mockMultiRepository = new Mock<IHttpMediaRepository<MediaBase>>();
            _searchService = new SearchService(_mockMovieRepository.Object, _mockTvRepository.Object, _mockMultiRepository.Object);
        }

        [Fact]
        public async Task FindMovieByNameAsync_ShouldReturnMovies()
        {
            // Arrange
            var movieResponse = new Response<Movie>
            {
                Results = MediaMock.GetMoviesMocks(),
                TotalResults = 2,
                TotalPages = 1,
                Page = 1
            };
            _mockMovieRepository.Setup(repo => repo.SearchMediaItemsByNameAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(movieResponse);

            // Act
            var result = await _searchService.FindMovieByNameAsync("Movie", 1);

            // Assert
            result.ShouldBe(movieResponse);
        }

        [Fact]
        public async Task FindTvByNameAsync_ShouldReturnTvSeries()
        {
            // Arrange
            var tvResponse = new Response<TvSerie>
            {
                Results = MediaMock.GetTvSeriesMocks(),
                TotalResults = 2,
                TotalPages = 1,
                Page = 1
            };
            _mockTvRepository.Setup(repo => repo.SearchMediaItemsByNameAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(tvResponse);

            // Act
            var result = await _searchService.FindTvByNameAsync("Tv", 1);

            // Assert
            result.ShouldBe(tvResponse);
        }

        [Fact]
        public async Task SearchMultiAsync_ShouldReturnMediaBase()
        {
            // Arrange
            var results = MediaMock.GetMultiMocks();
            var multiResponse = new Response<MediaBase>
            {
                Results = results,
                TotalResults = results.Count,
                TotalPages = 1,
                Page = 1
            };
            _mockMultiRepository.Setup(repo => repo.SearchMediaItemsByNameAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(multiResponse);

            // Act
            var result = await _searchService.SearchMultiAsync("Test", 1);

            // Assert
            result.ShouldBe(multiResponse);
        }

        [Fact]
        public async Task SearchMultiByMediaTasksAsync_ShouldReturnCombinedMediaBase()
        {
            // Arrange
            var movieResults = MediaMock.GetMoviesMocks();
            var tvResults = MediaMock.GetTvSeriesMocks();
            var movieResponse = new Response<Movie>
            {
                Results = movieResults,
                TotalResults = movieResults.Count,
                TotalPages = 1,
                Page = 1
            };
            var tvResponse = new Response<TvSerie>
            {
                Results = tvResults,
                TotalResults = tvResults.Count,
                TotalPages = 1,
                Page = 1
            };
            _mockMovieRepository.Setup(repo => repo.SearchMediaItemsByNameAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(movieResponse);
            _mockTvRepository.Setup(repo => repo.SearchMediaItemsByNameAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(tvResponse);

            // Act
            var result = await _searchService.SearchMultiByMediaTasksAsync("Test", 1);

            // Assert
            result?.Results.Count().ShouldBe(4);
            result?.Results.ShouldContain(media => media.Id == 1 && media.Title == "Movie 1");
            result?.Results.ShouldContain(media => media.Id == 2 && media.Title == "TvSerie 2");
            var movieResult = result?.Results.FirstOrDefault( m => m is Movie ) as Movie;
            movieResult?.IsAdult.ShouldBeTrue();
            movieResult?.Overview.ShouldNotBeNullOrEmpty();
            movieResult?.Title.ShouldNotBeNullOrEmpty();
            movieResult?.OriginalTitle.ShouldNotBeNullOrEmpty();
            movieResult?.PosterPath.ShouldNotBeNullOrEmpty();
            movieResult?.ReleaseDate.ShouldNotBeNullOrEmpty();
            result?.TotalResults.ShouldBe(4);
            result?.TotalPages.ShouldBe(1);
            result?.Page.ShouldBe(1);
        }
    }
}
