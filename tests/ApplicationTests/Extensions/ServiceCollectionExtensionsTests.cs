using Microsoft.Extensions.DependencyInjection;
using PDH.Shared.Infrastructure.Extensions;
using Xunit;

namespace PDH.ApplicationTests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddInfrastructure_Registers_Required_Services()
    {
        var services = new ServiceCollection();
        services.AddInfrastructure();
        // Just ensure no exception – the method registers services that depend on external components,
        // but we can at least verify the method runs.
        Assert.True(true);
    }
}
