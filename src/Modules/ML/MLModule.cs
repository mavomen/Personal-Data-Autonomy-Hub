using Microsoft.Extensions.DependencyInjection;
using MediatR;
using PDH.Shared.Kernel.Interfaces;

namespace PDH.Modules.ML;

public static class MLModule
{
    public static IServiceCollection AddML(this IServiceCollection services)
    {
        services.AddSingleton<ICategoryPredictor, CategoryPredictor>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MLModule).Assembly));
        return services;
    }
}
