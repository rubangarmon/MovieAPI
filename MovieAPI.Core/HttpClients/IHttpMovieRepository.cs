using MovieAPI.Core.Models;

namespace MovieAPI.Core.HttpClients
{
    public interface IHttpMovieRepository
    {
        Task<Response> GetByNameAsync(string name);
    }
}
