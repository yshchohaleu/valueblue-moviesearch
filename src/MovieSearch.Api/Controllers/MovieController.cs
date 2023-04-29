using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Application.Entities.Movies;
using MovieSearch.Application.Queries.SearchMovie;

namespace MovieSearch.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MovieController : ControllerBase
{
    private readonly ILogger<AdminController> _logger;
    private readonly IMediator _mediator;

    public MovieController(ILogger<AdminController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Movie))]
    public async Task<IActionResult> SearchAsync([FromQuery][Required] string title)
    {
        var result = await _mediator.Send(new SearchMovieQuery(title));
        return Ok(result);
    }
}