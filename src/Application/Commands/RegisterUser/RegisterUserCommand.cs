using MediatR;

namespace PDH.Application.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Password) : IRequest<Guid>;
