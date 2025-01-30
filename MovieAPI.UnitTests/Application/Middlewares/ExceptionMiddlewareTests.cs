using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MovieAPI.Application.Middlewares;
using MovieAPI.Core.Exceptions;
using Shouldly;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MovieAPI.UnitTests.Application.Middlewares;

public class ExceptionMiddlewareTests
{
    private readonly Mock<RequestDelegate> _mockNext;
    private readonly Mock<IMovieApiProblemDetailsFactory> _mockFactory;
    private readonly Mock<ILogger<Exception>> _mockLogger;
    private readonly Mock<IWebHostEnvironment> _mockEnv;
    private readonly DefaultHttpContext _httpContext;
    private readonly ExceptionMiddleware _middleware;

    public ExceptionMiddlewareTests()
    {
        _mockNext = new Mock<RequestDelegate>();
        _mockFactory = new Mock<IMovieApiProblemDetailsFactory>();
        _mockLogger = new Mock<ILogger<Exception>>();
        _mockEnv = new Mock<IWebHostEnvironment>();
        _httpContext = new DefaultHttpContext();
        _middleware = new ExceptionMiddleware(_mockNext.Object, _mockFactory.Object, _mockLogger.Object, _mockEnv.Object);
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallNextDelegate_WhenNoExceptionThrown()
    {
        // Arrange
        _mockNext.Setup(next => next(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

        // Act
        await _middleware.InvokeAsync(_httpContext);

        // Assert
        _mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleException_WhenExceptionThrown()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var traceId = Activity.Current?.Id ?? _httpContext.TraceIdentifier;
        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "An error occurred",
            Detail = exception.Message,
            Instance = traceId
        };

        _mockNext.Setup(next => next(It.IsAny<HttpContext>())).Throws(exception);
        _mockFactory.Setup(factory => factory.CreateProblemDetails(traceId, exception)).Returns(problemDetails);

        // Act
        await _middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
        _httpContext.Response.ContentType.ShouldBe("application/problem+json");

        //var responseBody = _httpContext.Response.Body;
        //responseBody.Seek(0, System.IO.SeekOrigin.Begin);
        //var responseText = new System.IO.StreamReader(responseBody).ReadToEnd();
        //var responseProblemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseText);

        //responseProblemDetails.ShouldNotBeNull();
        //responseProblemDetails.Status.ShouldBe(problemDetails.Status);
        //responseProblemDetails.Title.ShouldBe(problemDetails.Title);
        //responseProblemDetails.Detail.ShouldBe(problemDetails.Detail);
        //responseProblemDetails.Instance.ShouldBe(problemDetails.Instance);
    }
}
