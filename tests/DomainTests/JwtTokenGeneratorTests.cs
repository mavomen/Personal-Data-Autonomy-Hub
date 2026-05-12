using Microsoft.Extensions.Configuration;
using PDH.Shared.Infrastructure.Auth;
using Xunit;

namespace PDH.DomainTests;

public class JwtTokenGeneratorTests
{
    [Fact]
    public void GenerateToken_ReturnsValidToken()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"Jwt:Key", "A_very_long_secret_key_that_is_at_least_32_bytes_long!"},
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"},
            {"Jwt:ExpirationInMinutes", "60"}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var generator = new JwtTokenGenerator(configuration);
        var token = generator.GenerateToken(Guid.NewGuid(), "test@example.com");

        Assert.False(string.IsNullOrEmpty(token));
        Assert.Contains(".", token);
    }
}
