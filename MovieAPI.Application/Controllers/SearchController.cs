using Microsoft.AspNetCore.Mvc;
using MovieAPI.Core.HttpClients;

namespace MovieAPI.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IHttpMovieRepository _httpMovieService;

        public SearchController(IHttpMovieRepository httpMovieService)
        {
            _httpMovieService = httpMovieService;
        }

        [HttpGet]
        [Route("movies")]
        public async Task<IActionResult> GetMovies([FromQuery]string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest($"{name} cannot be empty");
            }
            var res = await _httpMovieService.GetMovieByNameAsync(name);
            if(res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpGet]
        [Route("series")]
        public async Task<IActionResult> GetSeries([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest($"{name} cannot be empty");
            }
            var res = await _httpMovieService.GetSerieTvByNameAsync(name);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
    }
}
