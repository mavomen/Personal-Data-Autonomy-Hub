using Microsoft.Extensions.DependencyInjection;
using PDH.Modules.ML;
using Xunit;

namespace PDH.ApplicationTests.Modules;

public class MLModuleTests
{
    [Fact]
    public void AddML_Registers_Predictor()
    {
        var services = new ServiceCollection();
        services.AddML();
        var provider = services.BuildServiceProvider();

        var predictor = provider.GetService<PDH.Shared.Kernel.Interfaces.ICategoryPredictor>();
        Assert.NotNull(predictor);
    }
}
