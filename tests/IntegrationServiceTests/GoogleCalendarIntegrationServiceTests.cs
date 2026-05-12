using RichardSzalay.MockHttp;
using PDH.Modules.Integrations;
using Xunit;
using System.Net.Http;
using System;

namespace PDH.IntegrationServiceTests;

public class GoogleCalendarIntegrationServiceTests
{
    [Fact]
    public async Task FetchActivities_CallsCalendarApi()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect("https://www.googleapis.com/calendar/v3/calendars/primary/events")
                .Respond("application/json", "{}");

        var httpClient = new HttpClient(mockHttp);
        httpClient.BaseAddress = new Uri("https://www.googleapis.com");  // ← fix

        var calendarClient = new GoogleCalendarClient(httpClient);
        var service = new GoogleCalendarIntegrationService(calendarClient);

        var result = await service.FetchActivitiesAsync(Guid.NewGuid());

        Assert.NotNull(result);
        mockHttp.VerifyNoOutstandingExpectation();
    }
}
