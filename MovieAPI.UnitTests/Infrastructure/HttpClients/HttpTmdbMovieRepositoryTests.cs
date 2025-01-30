using AutoMapper;
using Moq;
using Moq.Protected;
using MovieAPI.Core.Models;
using MovieAPI.Infrastructure.HttpClients;
using MovieAPI.ServiceModel.DTOs;
using MovieAPI.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace MovieAPI.UnitTests.Infrastructure.HttpClients;

public class HttpTmdbMovieRepositoryTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly Mock<IMapper> _mockMapper;
    private readonly HttpTmdbMovieRepository<Movie, MovieDTO> _movieRepository;
    private readonly HttpTmdbMovieRepository<MediaBase, MultiDTO> _multiRepository;

    public HttpTmdbMovieRepositoryTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/")
        };
        _mockMapper = new Mock<IMapper>();
        _movieRepository = new HttpTmdbMovieRepository<Movie, MovieDTO>(_httpClient, _mockMapper.Object);
        _multiRepository = new HttpTmdbMovieRepository<MediaBase, MultiDTO>(_httpClient, _mockMapper.Object);
    }

    [Fact]
    public async Task SearchMediaItemsByNameAsync_ShouldReturnMovies()
    {
        //Arrange
        var movieResponse = new Response<Movie>
        {
            Results = MediaMock.GetMoviesMocks(),
            TotalResults = 2,
            TotalPages = 1,
            Page = 1
        };

        var movieResponseDTO = new ResponseDTO<MovieDTO>
        {
            Results = MediaMock.GetMoviesMocks().ToDTO(),
            TotalResults = 2,
            TotalPages = 1,
            Page = 1
        };

        ConfigureMockHttpResponse(movieResponseDTO);

        _mockMapper
            .Setup(m => m.Map<Response<Movie>>(It.IsAny<ResponseDTO<MovieDTO>>()))
            .Returns(movieResponse);

        // Act
        var result = await _movieRepository.SearchMediaItemsByNameAsync("Test", 1);

        // Assert
        result.ShouldBe(movieResponse);
    }

    [Fact]
    public async Task SearchMediaItemsByNameAsync_ShouldReturnMultiMedia()
    {
        // Arrange
        var multiResponseDTO = new ResponseDTO<MultiDTO>
        {
            Results = new List<MultiDTO> { new MultiDTO { Id = 1, MediaType = "movie", Title = "Test Movie" } },
            TotalResults = 1,
            TotalPages = 1,
            Page = 1
        };
        var multiResponse = new Response<MediaBase>
        {
            Results = new List<MediaBase> { new Movie { Id = 1 } },
            TotalResults = 1,
            TotalPages = 1,
            Page = 1
        };

        ConfigureMockHttpResponse(multiResponseDTO);

        // Act
        var result = await _multiRepository.SearchMediaItemsByNameAsync("Test", 1);

        // Assert
        result.Results.Count().ShouldBe(1);
        result.Results.ShouldContain(media => media.Id == 1 && media.Title == "Test Movie");
        result.TotalResults.ShouldBe(1);
        result.TotalPages.ShouldBe(1);
        result.Page.ShouldBe(1);
    }

    [Fact]
    public async Task SearchMediaItemsByNameAsync_ShouldThrowExceptionOnHttpError()
    {
        // Arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        // Act & Assert
        await Should.ThrowAsync<HttpRequestException>(() => _movieRepository.SearchMediaItemsByNameAsync("Test", 1));
    }

    private void ConfigureMockHttpResponse<T>(ResponseDTO<T> responseDTO)
    {
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(responseDTO)
            });
    }
}
