using Microsoft.Extensions.DependencyInjection;
using PDH.Shared.Infrastructure.DomainEvents;
using PDH.Shared.Infrastructure.Outbox;
using PDH.Shared.Infrastructure.Persistence;
using PDH.Shared.Infrastructure.Repositories;
using PDH.Shared.Infrastructure.Auth;
using PDH.Shared.Kernel.Interfaces;

namespace PDH.Shared.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddHostedService<OutboxDispatcher>();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IOAuthTokenEncryptionService, OAuthTokenEncryptionService>();

        return services;
    }
}
