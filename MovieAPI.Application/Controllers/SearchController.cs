using Microsoft.AspNetCore.Mvc;
using MovieAPI.Core.HttpClients;
using MovieAPI.Core.Models;

namespace MovieAPI.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IHttpMediaRepository<Movie> _httpMovieService;
        private readonly IHttpMediaRepository<TvSerie> _httpTvSerieService;

        public SearchController(IHttpMediaRepository<Movie> httpMovieService, IHttpMediaRepository<TvSerie> httpTvSerieService)
        {
            _httpMovieService = httpMovieService;
            _httpTvSerieService = httpTvSerieService;
        }

        [HttpGet]
        [Route("movies")]
        public async Task<IActionResult> SearchMoviesByName([FromQuery]string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest($"{name} cannot be empty");
            }
            var res = await _httpMovieService.SearchMediaItemsByNameAsync(name);
            if(res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpGet]
        [Route("series")]
        public async Task<IActionResult> SearchSeriesByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest($"{name} cannot be empty");
            }
            var res = await _httpTvSerieService.SearchMediaItemsByNameAsync(name);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
    }
}
