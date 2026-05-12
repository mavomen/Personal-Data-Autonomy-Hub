using Microsoft.AspNetCore.Mvc;
using MediatR;
using PDH.Application.Commands.RegisterUser;
using PDH.Application.Queries.Login;

namespace PDH.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(Register), new { id = userId }, new { userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery query)
    {
        var token = await _mediator.Send(query);
        if (token == null)
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Invalid email or password"
            });

        return Ok(new { token });
    }
}
