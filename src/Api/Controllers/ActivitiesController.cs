using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using PDH.Application.Queries.GetActivities;
using PDH.Application.Commands.UpdateActivityCategory;

namespace PDH.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActivitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? cursor, [FromQuery] int limit = 20)
    {
        var query = new GetActivitiesQuery(cursor, limit);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/category")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest request)
    {
        var command = new UpdateActivityCategoryCommand(id, request.Category);
        await _mediator.Send(command);
        return NoContent();
    }
}

public record UpdateCategoryRequest(string Category);
