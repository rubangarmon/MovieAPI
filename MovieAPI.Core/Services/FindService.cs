using MovieAPI.Core.HttpClients;
using MovieAPI.Core.Models;

namespace MovieAPI.Core.Services;

public class FindService : IFindService
{
    private readonly IHttpMediaRepository<Movie> _httpMovieService;
    private readonly IHttpMediaRepository<TvSerie> _httpTvService;

    public FindService(IHttpMediaRepository<Movie> httpMovieService, IHttpMediaRepository<TvSerie> httpTvService)
    {
        _httpMovieService = httpMovieService;
        _httpTvService = httpTvService;
    }

    public async Task<Response<Movie>> FindMovieByNameAsync(string name)
    => await _httpMovieService.SearchMediaItemsByNameAsync(name);
    
    public async Task<Response<TvSerie>> FindTvByNameAsync(string name)
        => await _httpTvService.SearchMediaItemsByNameAsync(name);
    public async Task<IEnumerable<MediaBase>?> SearchMultiAsync(string query)
    {

        var movieSearchTask = _httpMovieService.SearchMediaItemsByNameAsync(query);
        var tvSearchTask = _httpTvService.SearchMediaItemsByNameAsync(query);

        await Task.WhenAll(movieSearchTask, tvSearchTask);
        var movieSearchResult = await movieSearchTask;
        var tvSearchResult = await tvSearchTask;

        var result = movieSearchResult.Results.Cast<MediaBase>().ToList();


        result.AddRange(tvSearchResult.Results);

        return result;

    }
}