namespace PDH.Modules.Integrations;

public class GoogleCalendarClient
{
    private readonly HttpClient _httpClient;

    public GoogleCalendarClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetCalendarEventsAsync(CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync("/calendar/v3/calendars/primary/events", ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(ct);
    }
}
