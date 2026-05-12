namespace PDH.Shared.Kernel;

public record ActivityImportedEvent(Guid ActivityId, DateTime OccurredOn) : IDomainEvent
{
    public ActivityImportedEvent(Guid activityId)
        : this(activityId, DateTime.UtcNow) { }
}
