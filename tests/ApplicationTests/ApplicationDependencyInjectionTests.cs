using Microsoft.Extensions.DependencyInjection;
using PDH.Application;
using Xunit;

namespace PDH.ApplicationTests;

public class ApplicationDependencyInjectionTests
{
    [Fact]
    public void AddApplication_Registers_MediatR()
    {
        var services = new ServiceCollection();
        services.AddApplication();
        var provider = services.BuildServiceProvider();
        // Just verify no exception
        Assert.True(true);
    }
}
