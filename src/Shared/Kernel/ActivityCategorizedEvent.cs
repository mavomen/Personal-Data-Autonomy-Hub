namespace PDH.Shared.Kernel;

public record ActivityCategorizedEvent(Guid ActivityId, string Category, DateTime OccurredOn) : IDomainEvent
{
    public ActivityCategorizedEvent(Guid activityId, string category)
        : this(activityId, category, DateTime.UtcNow) { }
}
