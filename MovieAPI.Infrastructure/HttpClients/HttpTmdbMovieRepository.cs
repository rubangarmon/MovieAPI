using AutoMapper;
using MovieAPI.Core.HttpClients;
using MovieAPI.Core.Models;
using MovieAPI.ServiceModel.DTOs;
using System.Text.Json;
using System.Web;

namespace MovieAPI.Infrastructure.HttpClients
{
    public class HttpTmdbMovieRepository<TMedia,TMediaDTO> : IHttpMediaRepository<TMedia> where TMedia : MediaBase
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string _url;
        public HttpTmdbMovieRepository(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;

            var typeMedia = typeof(TMedia);
            _url = typeMedia == typeof(Movie) ? "search/movie?" : "search/serie?";
        }

        public async Task<Response<TMedia>> GetMediaItemsByNameAsync(string name)
        {

            var dictionaryParameters = new Dictionary<string, string?>()
            {
                ["query"] = name,
                ["include_adult"] = false.ToString(),
                ["language"] = "en-US",
                ["page"] = "1"
            };
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            foreach (var pair in dictionaryParameters)
            {
                parameters.Add(pair.Key, pair.Value);
            }
            try
            {
                using HttpResponseMessage res = await _httpClient.GetAsync(_url + parameters.ToString());
                res.EnsureSuccessStatusCode();
                var body = res.Content.ReadAsStringAsync().Result;
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                var responseDTO = JsonSerializer.Deserialize<ResponseDTO<TMediaDTO>>(body, serializeOptions);
                var response = _mapper.Map<Response<TMedia>>(responseDTO);
                return response ?? new Response<TMedia>();

            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
