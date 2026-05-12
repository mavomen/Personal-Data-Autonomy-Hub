using Moq;
using RichardSzalay.MockHttp;
using PDH.Modules.Integrations;
using Xunit;
using System.Net.Http;
using System;

namespace PDH.IntegrationServiceTests;

public class GitHubIntegrationServiceTests
{
    [Fact]
    public async Task FetchActivities_CallsGitHubApi()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect("https://api.github.com/users/dummy/events")
                .Respond("application/json", "[]");

        var httpClient = new HttpClient(mockHttp);
        httpClient.BaseAddress = new Uri("https://api.github.com");  // ← fix

        var gitHubClient = new GitHubClient(httpClient);
        var service = new GitHubIntegrationService(gitHubClient);

        var result = await service.FetchActivitiesAsync(Guid.NewGuid());

        Assert.NotNull(result);
        mockHttp.VerifyNoOutstandingExpectation();
    }
}
