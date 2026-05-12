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

    // Modules that are allowed as shared dependencies
    private static readonly string[] AllowedSharedDependencies = new[]
    {
        "PDH.Modules.Activities",   // ActivityEvent is a core domain concept
        "PDH.Application"           // Application layer contracts
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
                if (AllowedSharedDependencies.Contains(other)) continue;

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
