using MovieAPI.Core.Models;

namespace MovieAPI.Core.HttpClients
{
    public interface IHttpMovieRepository
    {
        Task<Response<Movie>> GetMovieByNameAsync(string name);
        Task<Response<TvSerie>> GetSerieTvByNameAsync(string name);
    }
}
