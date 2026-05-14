using Xunit;

namespace PDH.IntegrationTests;

public class AuthControllerIntegrationTests
{
    [Fact(Skip = "Requires a running PostgreSQL instance with the configured credentials")]
    public void Register_Returns_Success() { }

    [Fact(Skip = "Requires a running PostgreSQL instance with the configured credentials")]
    public void Register_Without_Email_Returns_BadRequest() { }
}
