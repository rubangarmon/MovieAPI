using MovieAPI.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace MovieAPI.Application.Middlewares
{
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly IMovieApiProblemDetailsFactory _factory;
        private readonly ILogger<Exception> _logger;

        public ExceptionMiddleware(RequestDelegate next, IMovieApiProblemDetailsFactory factory, ILogger<Exception> logger)
        {
            _next = next;
            _factory = factory;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var problemDetails = _factory.CreateProblemDetails(ex);
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode =  problemDetails?.Status ?? (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }



            
    }
}
