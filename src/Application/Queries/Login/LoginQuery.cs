using MediatR;

namespace PDH.Application.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<string?>; // returns JWT token or null
