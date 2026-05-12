using PDH.Application.Interfaces;
using PDH.Modules.Activities;
using System.Text.Json;

namespace PDH.Modules.Integrations;

public class GoogleCalendarIntegrationService : IIntegrationService
{
    private readonly GoogleCalendarClient _client;
    public string Provider => "GoogleCalendar";

    public GoogleCalendarIntegrationService(GoogleCalendarClient client)
    {
        _client = client;
    }

    public async Task<IReadOnlyList<ActivityEvent>> FetchActivitiesAsync(Guid userId, CancellationToken ct = default)
    {
        var json = await _client.GetCalendarEventsAsync(ct);
        // Parse and map
        return new List<ActivityEvent>();
    }
}
