using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Application.ContractsModels;
using MovieAPI.Application.Extensions;
using MovieAPI.Core.Services;

namespace MovieAPI.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IFindService _findService;
    private readonly IValidator<MediaRequest> _validator;

    public SearchController(
        IFindService findService,
        IValidator<MediaRequest> validator)
    {
        _validator = validator;
        _findService = findService;
    }

    [HttpGet]
    [Route("movies")]
    public async Task<IActionResult> SearchMoviesByName([FromQuery] MediaRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid) return ValidationProblem(validationResult.SendErrosAsValidationProblem());
        var res = await _findService.FindMovieByNameAsync(request.Name);
        if (res == null) return NotFound();
        return Ok(res);
    }

    [HttpGet]
    [Route("series")]
    public async Task<IActionResult> SearchSeriesByName([FromQuery] MediaRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid) return ValidationProblem(validationResult.SendErrosAsValidationProblem());
        var res = await _findService.FindTvByNameAsync(request.Name);
        if (res == null) return NotFound();
        return Ok(res);
    }

    /// Search for movies and series using same request.Name value
    [HttpGet]
    [Route("multi")]
    public async Task<IActionResult> SearchMulti([FromQuery] MediaRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid) return ValidationProblem(validationResult.SendErrosAsValidationProblem());

        var res = await _findService.SearchMultiAsync(request.Name);
        if (res == null) return NotFound();
        return Ok(res);
    }
}