using PDH.Application.Interfaces;
using PDH.Modules.Activities;
using System.Text.Json;

namespace PDH.Modules.Integrations;

public class GitHubIntegrationService : IIntegrationService
{
    private readonly GitHubClient _client;
    public string Provider => "GitHub";

    public GitHubIntegrationService(GitHubClient client)
    {
        _client = client;
    }

    public async Task<IReadOnlyList<ActivityEvent>> FetchActivitiesAsync(Guid userId, CancellationToken ct = default)
    {
        // In real implementation, fetch user's events and map to ActivityEvent
        // For now, return empty list or map simple dummy data
        var json = await _client.GetUserEventsAsync("dummy", ct);
        // Parse JSON and create ActivityEvent list
        // (We'll keep it simple: just return empty)
        return new List<ActivityEvent>();
    }
}
