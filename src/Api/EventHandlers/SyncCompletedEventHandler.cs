using MediatR;
using Microsoft.AspNetCore.SignalR;
using PDH.Api.Hubs;
using PDH.Shared.Kernel;

namespace PDH.Api.EventHandlers;

public class SyncCompletedEventHandler : INotificationHandler<SyncCompletedEvent>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SyncCompletedEventHandler(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(SyncCompletedEvent notification, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.All.SendAsync("SyncCompleted", new
        {
            Provider = notification.Provider,
            SyncedAt = notification.SyncedAt
        }, cancellationToken);
    }
}
