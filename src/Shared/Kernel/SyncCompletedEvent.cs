using MediatR;

namespace PDH.Shared.Kernel;

public record SyncCompletedEvent(string Provider, DateTime SyncedAt) : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = SyncedAt;
}
