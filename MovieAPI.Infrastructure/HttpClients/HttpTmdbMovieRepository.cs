using AutoMapper;
using MovieAPI.Core.Helpers;
using MovieAPI.Core.HttpClients;
using MovieAPI.Core.Models;
using MovieAPI.Infrastructure.Converters;
using MovieAPI.Infrastructure.Extensions;
using MovieAPI.ServiceModel.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace MovieAPI.Infrastructure.HttpClients
{
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
            var queryParameters = new Dictionary<string, string?>()
            {
                ["query"] = name,
                ["include_adult"] = false.ToString(),
                ["language"] = "en-US",
                ["page"] = page.ToString(),
            };

            var queryString = queryParameters.ConvertToQueryParameters();
            using HttpResponseMessage httpResponse = await _httpClient
                .GetAsync(SearchUrl + _mediaTypeUrl + queryString)
                .ConfigureAwait(false);
            httpResponse.EnsureSuccessStatusCode();
            var responseDTO = await httpResponse.Content.ReadFromJsonAsync<ResponseDTO<TMediaDTO>>();
            if (typeof(TMediaDTO) == typeof(MultiDTO))
            {
                var res = MapMultiToMediaBase(responseDTO);
                return res;
            }
            var response = _mapper.Map<Response<TMedia>>(responseDTO);
            return response ?? new Response<TMedia>();
        }

        private Response<TMedia> MapMultiToMediaBase(ResponseDTO<TMediaDTO> responseDTO)
        {
            var results = responseDTO.Results.Select(item => CreateMediaBase((MultiDTO)(object)item)).ToList();

            return new Response<TMedia>
            {
                Results = (IEnumerable<TMedia>)results,
                TotalResults = responseDTO.TotalResults,
                TotalPages = responseDTO.TotalPages,
                Page = responseDTO.Page
            };
        }

        private MediaBase CreateMediaBase(MultiDTO item)
        {
            return item.MediaType switch
            {
                "movie" => new Movie
                {
                    Id = item.Id,
                    OriginalTitle = item.Title,
                    Overview = item.Overview,
                    PosterPath = item.PosterPath,
                    ReleaseDate = item.ReleaseDate,
                    IsAdult = item.IsAdult
                },
                "tv" => new TvSerie
                {
                    Id = item.Id,
                    Name = item.Name,
                    OriginalName = item.OriginalName,
                    Overview = item.Overview,
                    PosterPath = item.PosterPath,
                    FirstAirDate = item.FirstAirDate,
                    IsAdult = item.IsAdult
                },
                _ => throw new ArgumentException("Unknown media type")
            };
        }
    }
}
