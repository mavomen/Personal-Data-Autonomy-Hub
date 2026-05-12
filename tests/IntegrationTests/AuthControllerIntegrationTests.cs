using Microsoft.AspNetCore.Mvc.Testing;
using PDH.Api;
using System.Net.Http.Json;
using Xunit;

namespace PDH.IntegrationTests;

public class AuthControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_Returns_Success()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register", new
        {
            email = "integration@test.com",
            password = "TestPass123!"
        });

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Register_Without_Email_Returns_BadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register", new
        {
            password = "TestPass123!"
        });

        Assert.False(response.IsSuccessStatusCode);
    }
}
