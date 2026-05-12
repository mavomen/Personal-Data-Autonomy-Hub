using MediatR;

namespace PDH.Application.Commands.ConnectIntegration;

public record ConnectIntegrationCommand(string Provider, string AccessToken, string RefreshToken, DateTime ExpiresAt) : IRequest;
