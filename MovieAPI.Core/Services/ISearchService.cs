using MovieAPI.Core.Models;

namespace MovieAPI.Core.Services;

public interface ISearchService
{
    public Task<Response<MediaBase>?> SearchMultiAsync(string name, int page);
    public Task<Response<Movie>> FindMovieByNameAsync(string name, int page);
    public Task<Response<TvSerie>> FindTvByNameAsync(string name, int page);
    public Task<Response<MediaBase>> SearchMultiByMediaTasksAsync(string name, int page);
}
 