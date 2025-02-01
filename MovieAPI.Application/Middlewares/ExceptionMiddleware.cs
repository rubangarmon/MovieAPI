using MovieAPI.Core.Exceptions;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace MovieAPI.Application.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, IMovieApiProblemDetailsFactory factory,
        ILogger<Exception> logger, IWebHostEnvironment env)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                logger.LogError(ex, "tracedId: {traceId}", traceId);
                await HandleExceptionAsync(context, ex, traceId);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, string traceId)
        {

            var problemDetails = factory.CreateProblemDetails(traceId, ex);
            context.Response.StatusCode = problemDetails?.Status ?? (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/problem+json";

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
            await context.Response.Body.FlushAsync();
            //await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));

        }
    }
}
