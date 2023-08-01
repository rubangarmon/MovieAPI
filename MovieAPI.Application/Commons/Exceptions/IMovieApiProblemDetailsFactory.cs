using Microsoft.AspNetCore.Mvc;

namespace MovieAPI.Core.Exceptions
{
    public interface IMovieApiProblemDetailsFactory
    {
        public ProblemDetails CreateProblemDetails(Exception exception);
    }
}
