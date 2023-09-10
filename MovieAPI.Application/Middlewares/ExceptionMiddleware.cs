using MovieAPI.Core.Exceptions;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace MovieAPI.Application.Middlewares
{
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly IMovieApiProblemDetailsFactory _factory;
        private readonly ILogger<Exception> _logger;
        private readonly IWebHostEnvironment _env; //TODO: env variable

        public ExceptionMiddleware(RequestDelegate next, IMovieApiProblemDetailsFactory factory, 
            ILogger<Exception> logger, IWebHostEnvironment env)
        {
            _next = next;
            _factory = factory;
            _logger = logger;
            _env = env;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                _logger.LogError(ex, "tracedId: {traceId}", traceId);
                await HandleExceptionAsync(context, ex, traceId);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, string traceId)
        {
            var problemDetails = _factory.CreateProblemDetails(traceId,ex);
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode =  problemDetails?.Status ?? (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }



            
    }
}
