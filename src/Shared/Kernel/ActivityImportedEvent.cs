using MediatR;

namespace PDH.Shared.Kernel;

public record ActivityImportedEvent : IDomainEvent, INotification
{
    public Guid ActivityId { get; }
    public DateTime OccurredOn { get; }

    public ActivityImportedEvent(Guid activityId)
    {
        ActivityId = activityId;
        OccurredOn = DateTime.UtcNow;
    }
}
