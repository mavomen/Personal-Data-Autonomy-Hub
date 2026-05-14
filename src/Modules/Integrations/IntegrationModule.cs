using Microsoft.Extensions.DependencyInjection;
using PDH.Application.Interfaces;

namespace PDH.Modules.Integrations;

public static class IntegrationModule
{
    public static IServiceCollection AddIntegrations(this IServiceCollection services)
    {
        // HttpClient registrations
        services.AddHttpClient<GitHubClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.github.com");
            client.DefaultRequestHeaders.Add("User-Agent", "PersonalDataHub");
        });

        services.AddHttpClient<GoogleCalendarClient>(client =>
        {
            client.BaseAddress = new Uri("https://www.googleapis.com");
        });

        // Integration services
        services.AddScoped<IIntegrationService, GitHubIntegrationService>();
        services.AddScoped<IIntegrationService, GoogleCalendarIntegrationService>();
        services.AddScoped<IIntegrationFactory, IntegrationFactory>();

        return services;
    }
}
