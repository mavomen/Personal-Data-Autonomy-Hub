using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using PDH.Application.Commands.ConnectIntegration;

namespace PDH.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IntegrationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public IntegrationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{provider}/connect")]
    public async Task<IActionResult> Connect(string provider, [FromBody] ConnectIntegrationRequest request)
    {
        var command = new ConnectIntegrationCommand(provider, request.AccessToken, request.RefreshToken, request.ExpiresAt);
        await _mediator.Send(command);
        return NoContent();
    }
}

public record ConnectIntegrationRequest(string AccessToken, string RefreshToken, DateTime ExpiresAt);
