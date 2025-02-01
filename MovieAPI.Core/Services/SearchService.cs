using MovieAPI.Core.HttpClients;
using MovieAPI.Core.Models;

namespace MovieAPI.Core.Services;

public class SearchService(
    IHttpMediaRepository<Movie> httpMovieService, 
    IHttpMediaRepository<TvSerie> httpTvService, 
    IHttpMediaRepository<MediaBase> httpMultiService) : ISearchService
{
    private readonly IHttpMediaRepository<Movie> _httpMovieService = httpMovieService;
    private readonly IHttpMediaRepository<TvSerie> _httpTvService = httpTvService;
    private readonly IHttpMediaRepository<MediaBase> _httpMultiService = httpMultiService;
    private const int PageSize = 20;

    public async Task<Response<Movie>> FindMovieByNameAsync(string name, int page)
        => await _httpMovieService.SearchMediaItemsByNameAsync(name, page);
    
    public async Task<Response<TvSerie>> FindTvByNameAsync(string name, int page)
        => await _httpTvService.SearchMediaItemsByNameAsync(name, page);

    public async Task<Response<MediaBase>?> SearchMultiAsync(string name, int page = 1)
    {
        var response = await _httpMultiService.SearchMediaItemsByNameAsync(name, page);
        return response;
    }

    public async Task<Response<MediaBase>> SearchMultiByMediaTasksAsync(string name, int page = 1)
    {
        var movieSearchTask = _httpMovieService.SearchMediaItemsByNameAsync(name, page);
        var tvSearchTask = _httpTvService.SearchMediaItemsByNameAsync(name, page);

        await Task.WhenAll(movieSearchTask, tvSearchTask);
        var movieSearchResponseResult = await movieSearchTask;
        var tvSearchResponseResult = await tvSearchTask;

        var result = movieSearchResponseResult.Results
            .Cast< MediaBase>()
            .Concat(tvSearchResponseResult.Results)
            .OrderBy(media => media.Title)
            .ToList();
        
        var totalResults = movieSearchResponseResult.TotalResults + tvSearchResponseResult.TotalResults;
        var response = new Response<MediaBase>
        {
            Results = result,
            TotalResults = totalResults,
            TotalPages = (int)Math.Ceiling((double)totalResults / PageSize),
            Page = page
        };
        return response;
    }
}