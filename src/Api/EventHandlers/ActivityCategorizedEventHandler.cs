using MediatR;
using Microsoft.AspNetCore.SignalR;
using PDH.Api.Hubs;
using PDH.Shared.Kernel;

namespace PDH.Api.EventHandlers;

public class ActivityCategorizedEventHandler : INotificationHandler<ActivityCategorizedEvent>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public ActivityCategorizedEventHandler(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(ActivityCategorizedEvent notification, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.All.SendAsync("ActivityCategorized", new
        {
            ActivityId = notification.ActivityId,
            Category = notification.Category
        }, cancellationToken);
    }
}
