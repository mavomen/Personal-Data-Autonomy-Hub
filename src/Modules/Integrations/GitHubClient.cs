namespace PDH.Modules.Integrations;

public class GitHubClient
{
    private readonly HttpClient _httpClient;

    public GitHubClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetUserEventsAsync(string username, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync($"/users/{username}/events", ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(ct);
    }
}
