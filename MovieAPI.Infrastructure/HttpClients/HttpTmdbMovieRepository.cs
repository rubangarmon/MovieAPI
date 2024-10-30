using AutoMapper;
using MovieAPI.Core.Helpers;
using MovieAPI.Core.HttpClients;
using MovieAPI.Core.Models;
using MovieAPI.Infrastructure.Extensions;
using MovieAPI.ServiceModel.DTOs;
using System.Net.Http.Json;

namespace MovieAPI.Infrastructure.HttpClients
{
    public class HttpTmdbMovieRepository<TMedia, TMediaDTO> : IHttpMediaRepository<TMedia> where TMedia : MediaBase
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string _mediaTypeUrl;
        public HttpTmdbMovieRepository(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;

            _mediaTypeUrl = MediaTypeHelper.DetermineMediaTypeUrl<TMedia>();
        }

        public async Task<Response<TMedia>> SearchMediaItemsByNameAsync(string name)
        {
            var searchUrl = "search/";
            var queryParameters = new Dictionary<string, string?>()
            {
                ["query"] = name,
                ["include_adult"] = false.ToString(),
                ["language"] = "en-US",
                ["page"] = "1"
            };

            var queryString = queryParameters.ConvertToQueryParameters();
            using HttpResponseMessage httpResponse = await _httpClient
                .GetAsync(searchUrl + _mediaTypeUrl + queryString)
                .ConfigureAwait(false);
            httpResponse.EnsureSuccessStatusCode();
            var responseDTO = await httpResponse.Content.ReadFromJsonAsync<ResponseDTO<TMediaDTO>>();
            var response = _mapper.Map<Response<TMedia>>(responseDTO);
            return response ?? new Response<TMedia>();
        }


    }
}
