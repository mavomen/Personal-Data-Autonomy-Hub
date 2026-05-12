using MediatR;

namespace PDH.Shared.Kernel;

public record ActivityCategorizedEvent : IDomainEvent, INotification
{
    public Guid ActivityId { get; }
    public string Category { get; }
    public DateTime OccurredOn { get; }

    public ActivityCategorizedEvent(Guid activityId, string category)
    {
        ActivityId = activityId;
        Category = category;
        OccurredOn = DateTime.UtcNow;
    }
}
