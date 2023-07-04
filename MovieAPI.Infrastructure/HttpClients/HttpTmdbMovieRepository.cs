using AutoMapper;
using MovieAPI.Core.HttpClients;
using MovieAPI.Core.Models;
using MovieAPI.ServiceModel.DTOs;
using System.Text.Json;
using System.Web;

namespace MovieAPI.Infrastructure.HttpClients
{
    public class HttpTmdbMovieRepository : IHttpMovieRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public HttpTmdbMovieRepository(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }
        public async Task<Response<Movie>> GetMovieByNameAsync(string name)
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
                using HttpResponseMessage res = await _httpClient.GetAsync("search/movie?" + parameters.ToString());
                res.EnsureSuccessStatusCode();
                var body = res.Content.ReadAsStringAsync().Result;
                //var responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(body, new JsonSerializerSettings()
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                var responseDTO = JsonSerializer.Deserialize<ResponseDTO<MovieDTO>>(body, serializeOptions);
                var response = _mapper.Map<Response<Movie>>(responseDTO);
                return response ?? new Response<Movie>();

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<Response<TvSerie>> GetSerieTvByNameAsync(string name)
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
                using HttpResponseMessage res = await _httpClient.GetAsync("search/serie?" + parameters.ToString());
                res.EnsureSuccessStatusCode();
                var body = res.Content.ReadAsStringAsync().Result;
                var responseDTO = JsonSerializer.Deserialize<ResponseDTO<TvSerieDTO>>(body);
                var response = _mapper.Map<Response<TvSerie>>(responseDTO);
                return response ?? new Response<TvSerie>();

            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
