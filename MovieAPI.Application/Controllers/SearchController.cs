using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Application.ContractsModels;
using MovieAPI.Application.Extensions;
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
        private readonly IValidator<MediaRequest> _validator;

        public SearchController(
            IHttpMediaRepository<Movie> httpMovieService, 
            IHttpMediaRepository<TvSerie> httpTvSerieService,
            IValidator<MediaRequest> validator)
        {
            _httpMovieService = httpMovieService;
            _httpTvSerieService = httpTvSerieService;
            _validator = validator;
        }

        [HttpGet]
        [Route("movies")]
        public async Task<IActionResult> SearchMoviesByName([FromQuery] MediaRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(validationResult.SendErrosAsValidationProblem());
            }
            var res = await _httpMovieService.SearchMediaItemsByNameAsync(request.Name);
            if(res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpGet]
        [Route("series")]
        public async Task<IActionResult> SearchSeriesByName([FromQuery] MediaRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(validationResult.SendErrosAsValidationProblem());
            }
            var res = await _httpTvSerieService.SearchMediaItemsByNameAsync(request.Name);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
    }
}
