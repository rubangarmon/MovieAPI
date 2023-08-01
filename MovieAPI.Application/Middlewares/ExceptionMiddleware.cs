using MovieAPI.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace MovieAPI.Application.Middlewares
{
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly IMovieApiProblemDetailsFactory _factory;

        public ExceptionMiddleware(RequestDelegate next, IMovieApiProblemDetailsFactory factory)
        {
            _next = next;
            _factory = factory;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
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
