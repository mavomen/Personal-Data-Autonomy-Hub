using PDH.Shared.Kernel;

namespace PDH.Modules.Activities;

public class ActivityEvent : Entity, IAggregateRoot
{
    public string Title { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string SourceProvider { get; private set; }
    public ActivityCategory Category { get; private set; }

    private ActivityEvent()
    {
        Title = null!;
        SourceProvider = null!;
    }

    public ActivityEvent(Guid id, string title, DateTime timestamp, string sourceProvider)
    {
        Id = id;
        Title = title ?? throw new DomainException("Title is required");
        Timestamp = timestamp;
        SourceProvider = sourceProvider ?? throw new DomainException("Source is required");
        Category = ActivityCategory.DeepWork; // default
    }

    public void UpdateCategory(ActivityCategory newCategory)
    {
        Category = newCategory;
    }
}
