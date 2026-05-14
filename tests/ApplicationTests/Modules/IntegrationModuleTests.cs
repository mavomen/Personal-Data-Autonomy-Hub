using Microsoft.Extensions.DependencyInjection;
using PDH.Modules.Integrations;
using Xunit;

namespace PDH.ApplicationTests.Modules;

public class IntegrationModuleTests
{
    [Fact]
    public void AddIntegrations_Registers_Required_Services()
    {
        var services = new ServiceCollection();
        services.AddIntegrations();
        var provider = services.BuildServiceProvider();

        var factory = provider.GetService<PDH.Application.Interfaces.IIntegrationFactory>();
        Assert.NotNull(factory);
    }
}
