using PDH.Application.Interfaces;

namespace PDH.Modules.Integrations;

public class IntegrationFactory : IIntegrationFactory
{
    private readonly IEnumerable<IIntegrationService> _services;

    public IntegrationFactory(IEnumerable<IIntegrationService> services)
    {
        _services = services;
    }

    public IIntegrationService GetService(string provider)
    {
        var service = _services.FirstOrDefault(s =>
            s.Provider.Equals(provider, StringComparison.OrdinalIgnoreCase));
        if (service == null)
            throw new ArgumentException($"Unknown provider: {provider}");
        return service;
    }
}
