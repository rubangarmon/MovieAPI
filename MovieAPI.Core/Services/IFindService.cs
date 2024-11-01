using MovieAPI.Core.Models;

namespace MovieAPI.Core.Services;

public interface IFindService
{
    public Task<Response<MediaBase>?> SearchMultiAsync(string name, int page);
    public Task<Response<Movie>> FindMovieByNameAsync(string name, int page);
    public Task<Response<TvSerie>> FindTvByNameAsync(string name, int page);
}
 