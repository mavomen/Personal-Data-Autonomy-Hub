using Xunit;

namespace PDH.IntegrationTests;

public class DatabaseIntegrationTests
{
    [Fact(Skip = "Requires Docker to be running")]
    public void CanApplyMigrationAndQueryDatabase() { }
}
