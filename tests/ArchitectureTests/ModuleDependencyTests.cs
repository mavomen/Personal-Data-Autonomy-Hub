using NetArchTest.Rules;
using System.Reflection;
using Xunit;

namespace PDH.ArchitectureTests;

public class ModuleDependencyTests
{
    private static readonly string[] ModuleProjects = new[]
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
        foreach (var module in ModuleProjects)
        {
            var types = Types.InAssembly(Assembly.Load(module));

            var result = types
                .That()
                .ResideInNamespace($"{module}")
                .ShouldNot()
                .HaveDependencyOnAny(ModuleProjects.Where(m => m != module).ToArray())
                .GetResult();

            Assert.True(result.IsSuccessful, $"Module {module} has a forbidden dependency: {result.FailingTypeNames}");
        }
    }
}
