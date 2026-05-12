using PDH.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace PDH.Modules.Integrations;

public class IntegrationFactory : IIntegrationFactory
{
    private readonly IServiceProvider _serviceProvider;

    public IntegrationFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IIntegrationService GetService(string provider)
    {
        return provider switch
        {
            "GitHub" => _serviceProvider.GetRequiredService<GitHubIntegrationService>(),
            "GoogleCalendar" => _serviceProvider.GetRequiredService<GoogleCalendarIntegrationService>(),
            _ => throw new ArgumentException($"Unknown provider: {provider}")
        };
    }
}
