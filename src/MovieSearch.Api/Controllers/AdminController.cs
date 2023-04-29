using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Api.Security;
using MovieSearch.Application.Entities.Requests;
using MovieSearch.Application.Queries.GetSearchRequest;
using MovieSearch.Application.Queries.GetSearchRequests;
using MovieSearch.Application.Queries.GetSearchRequestStatistics;
using MovieSearch.Application.Requests.DeleteSearchRequest;

namespace MovieSearch.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiKey]
public class AdminController : ControllerBase
{
    private readonly ILogger<AdminController> _logger;
    private readonly IMediator _mediator;

    public AdminController(ILogger<AdminController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpDelete("SearchRequest/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync([Required] string id)
    {
        await _mediator.Send(new DeleteSearchRequest(id));
        return NoContent();
    }

    [HttpGet("SearchRequests")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchRequests))]
    public async Task<IActionResult> GetItemsAsync(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10)
    {
        var response = await _mediator.Send(new GetSearchRequestsQuery(from, to, page, pageSize));
        return Ok(response);
    }
    
    [HttpGet("SearchRequests/Statistics")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DailyRequestStatistics))]
    public async Task<IActionResult> GetStatisticsAsync([FromQuery][Required] string date)
    {
        if (DateTime.TryParse(date, out DateTime parsedDate))
        {
            var response = await _mediator.Send(new GetSearchRequestStatisticsQuery(parsedDate));
            return Ok(response);
        }

        return BadRequest();
    }
    
    [HttpGet("SearchRequest/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchRequest))]
    public async Task<IActionResult> GetAsync([FromRoute][Required] string id)
    {
        var response = await _mediator.Send(new GetSearchRequestQuery(id));
        return Ok(response);
    }
}