using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace PDH.ArchitectureTests;

public class ModuleDependencyTests
{
    private static readonly string[] ModuleNames = new[]
    {
        "PDH.Modules.Identity",
        "PDH.Modules.Activities",
        "PDH.Modules.Integrations",
        "PDH.Modules.ML",
        "PDH.Modules.Chat",
        "PDH.Modules.Analytics"
    };

    [Fact]
    public void Modules_Should_Not_Depend_On_Each_Other()
    {
        foreach (var moduleName in ModuleNames)
        {
            var assembly = Assembly.Load(moduleName);
            foreach (var other in ModuleNames)
            {
                if (other == moduleName) continue;

                var result = Types
                    .InAssembly(assembly)
                    .That()
                    .ResideInNamespace(moduleName)
                    .ShouldNot()
                    .HaveDependencyOn(other)
                    .GetResult();

                Assert.True(result.IsSuccessful,
                    $"Module {moduleName} should not depend on {other}");
            }
        }
    }
}
