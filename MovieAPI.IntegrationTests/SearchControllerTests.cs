using MovieAPI.Core.Models;
using MovieAPI.IntegrationTests.Utils;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace MovieAPI.IntegrationTests;

public class SearchControllerTests
{
    private readonly HttpClient _client;
    private readonly MockHttpMessageHandler _mockHttpMessageHandler;

    public SearchControllerTests()
    {
        _mockHttpMessageHandler = new MockHttpMessageHandler();
        var application = new MovieApiWebApplicationFactory(_mockHttpMessageHandler);
        _client = application.CreateClient();
    }

    private void RegisterMockResponse(string url, string filePath, HttpStatusCode statusCode)
    {
        _mockHttpMessageHandler.RegisterResponse(url, filePath, statusCode);
    }

    [Fact]
    public async Task GetMovies_Page1_ShouldReturnMovies()
    {
        // Arrange
        RegisterMockResponse(
            "/3/search/movie?query=lord+of+the+rings&include_adult=False&language=en-US&page=1", 
            "MockResponses/GetMoviesResponsePage1.json", 
            HttpStatusCode.OK
        );
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/Search/movies?Name=lord%20of%20the%20rings&Page=1");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var movies = await response.Content.ReadFromJsonAsync<Response<Movie>>();
        movies.ShouldNotBeNull();
        movies.Results.ShouldNotBeEmpty();
        movies.Results.Count().ShouldBe(2);
        movies.Page.ShouldBe(1);
        movies.TotalPages.ShouldBe(2);
        movies.TotalResults.ShouldBe(4);
        movies.Results.First().Title.ShouldBe("Test Movie 1");
    }

    [Fact]
    public async Task GetMovies_Page2_ShouldReturnMovies()
    {
        // Arrange
        RegisterMockResponse("/3/search/movie?query=lord+of+the+rings&include_adult=False&language=en-US&page=2", "MockResponses/GetMoviesResponsePage2.json", HttpStatusCode.OK);
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/Search/movies?Name=lord%20of%20the%20rings&Page=2");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var movies = await response.Content.ReadFromJsonAsync<Response<Movie>>();
        movies.ShouldNotBeNull();
        movies.Results.ShouldNotBeEmpty();
        movies.Results.Count().ShouldBe(2);
        movies.Page.ShouldBe(2);
        movies.TotalPages.ShouldBe(2);
        movies.TotalResults.ShouldBe(4);
        movies.Results.First().Title.ShouldBe("Test Movie 3");
    }
}
