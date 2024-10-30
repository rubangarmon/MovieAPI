using MovieAPI.Core.Models;

namespace MovieAPI.Core.Services;

public interface IFindService
{
    public Task<IEnumerable<MediaBase>?> SearchMultiAsync(string query);
    public Task<Response<Movie>> FindMovieByNameAsync(string name);
    public Task<Response<TvSerie>> FindTvByNameAsync(string name);
}
 