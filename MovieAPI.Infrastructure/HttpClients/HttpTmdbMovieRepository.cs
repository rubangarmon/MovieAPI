using AutoMapper;
using MovieAPI.Core.Helpers;
using MovieAPI.Core.HttpClients;
using MovieAPI.Core.Models;
using MovieAPI.Core.Utils;
using MovieAPI.Infrastructure.Extensions;
using MovieAPI.Infrastructure.MappingProfiles;
using MovieAPI.ServiceModel.DTOs;
using System.Net.Http.Json;

namespace MovieAPI.Infrastructure.HttpClients;

public class HttpTmdbMovieRepository<TMedia, TMediaDTO> : IHttpMediaRepository<TMedia> where TMedia : MediaBase
{
    private const string SearchUrl = "search/";
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly string _mediaTypeUrl;

    public HttpTmdbMovieRepository(HttpClient httpClient, IMapper mapper)
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _mediaTypeUrl = MediaTypeHelper.DetermineMediaTypeUrl<TMedia>();
    }

    public async Task<Response<TMedia>> SearchMediaItemsByNameAsync(string name, int page)
    {
        var queryString = QueryParameterFactory
            .CreateQueryParameterByName(name, page)
            .ConvertToQueryParameters();
        using HttpResponseMessage httpResponse = await _httpClient
            .GetAsync(SearchUrl + _mediaTypeUrl + queryString)
            .ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        var responseDTO = await httpResponse.Content.ReadFromJsonAsync<ResponseDTO<TMediaDTO>>();
        if (IsMulti())
            return MapMulti(responseDTO);
        var response = _mapper.Map<Response<TMedia>>(responseDTO);
        return response ?? new Response<TMedia>();
    }

    private static bool IsMulti() => typeof(TMediaDTO) == typeof(MultiDTO);
    private static Response<TMedia> MapMulti(ResponseDTO<TMediaDTO> responseDTO)
        => MultiMapper.MapMultiToMediaBase(responseDTO as ResponseDTO<MultiDTO>) as Response<TMedia>;
}
