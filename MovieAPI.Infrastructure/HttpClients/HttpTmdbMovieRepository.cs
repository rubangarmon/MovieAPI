using AutoMapper;
using MovieAPI.Core.HttpClients;
using MovieAPI.Core.Models;
using MovieAPI.ServiceModel.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<Response> GetByNameAsync(string name)
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
                var responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(body);
                var response = _mapper.Map<Response>(responseDTO);
                return response ?? new Response();

            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
