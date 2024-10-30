using Microsoft.AspNetCore.Mvc;
using MovieAPI.Core.Exceptions;
using System.Net;

namespace MovieAPI.Application.Commons.Exceptions
{
    public class MovieApiProblemDetailsFactory : IMovieApiProblemDetailsFactory
    {
        private delegate ProblemDetails HandleException(string traceId, Exception ex);
        private readonly IDictionary<Type, HandleException> _exceptionHandlers;

        public MovieApiProblemDetailsFactory()
        {
            _exceptionHandlers = new Dictionary<Type, HandleException>
            {
                {typeof(NotFoundException),  HandleNotFoundException}
            };

        }

        public ProblemDetails CreateProblemDetails(string traceId, Exception exception)
        {
            Type type = exception.GetType();
            if (_exceptionHandlers.TryGetValue(type, out HandleException? value))
            {
                return value.Invoke(traceId,exception);
            }
            return CreateRawProblemDetails(
                traceId,
                exception,
                "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                HttpStatusCode.InternalServerError,
                "We're sorry, but something went wrong.");
        }

        private static ProblemDetails CreateRawProblemDetails(string traceId,Exception exception, string type, HttpStatusCode httpStatusCode, string title)
        {
            return new()
            {
                Type = type,
                Status = (int)httpStatusCode,
                Title = title,
                Detail = exception.Message,
                Extensions =
                {
                    { "traceId", traceId }
                }
            };

        }
        private ProblemDetails HandleNotFoundException(string traceId, Exception ex)
            => CreateRawProblemDetails(
                traceId,
                ex,
                "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                HttpStatusCode.NotFound,
                "The specified resource was not found."); //TODO: ex.message
    }
}
