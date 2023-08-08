using Microsoft.AspNetCore.Mvc;
using MovieAPI.Core.Exceptions;
using System.Net;

namespace MovieAPI.Application.Commons.Exceptions
{
    public class MovieApiProblemDetailsFactory : IMovieApiProblemDetailsFactory
    {
        private readonly IDictionary<Type, Func<Exception, ProblemDetails>> _exceptionHandlers;

        public MovieApiProblemDetailsFactory()
        {
            _exceptionHandlers = new Dictionary<Type, Func<Exception, ProblemDetails>>
            {
                {typeof(NotFoundException),  HandleNotFoundException}
            };

        }

        public ProblemDetails CreateProblemDetails(Exception exception)
        {
            Type type = exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                return _exceptionHandlers[type].Invoke(exception);
            }

            return new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "The specified resource was not found.",
                Detail = exception.Message
            };

        }

        private ProblemDetails HandleNotFoundException(Exception ex)
        {
            return new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Status = (int)HttpStatusCode.NotFound,
                Title = "The specified resource was not found.",
                Detail = ex.Message
            };
        }


    }
}
