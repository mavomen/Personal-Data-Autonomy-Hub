using PDH.Modules.Activities;
using PDH.Shared.Kernel;
using Xunit;

namespace PDH.DomainTests;

public class ActivityEventTests
{
    [Fact]
    public void CreateActivityEvent_WithRequiredFields_SetsProperties()
    {
        var activity = new ActivityEvent(Guid.NewGuid(), "Coding session", DateTime.UtcNow, "GitHub");

        Assert.Equal("Coding session", activity.Title);
        Assert.Equal("GitHub", activity.SourceProvider);
        Assert.Equal(ActivityCategory.DeepWork, activity.Category);
    }

    [Fact]
    public void UpdateCategory_ChangesCategory()
    {
        var activity = new ActivityEvent(Guid.NewGuid(), "Lunch", DateTime.UtcNow, "GoogleCalendar");
        activity.UpdateCategory(ActivityCategory.Social);

        Assert.Equal(ActivityCategory.Social, activity.Category);
    }

    [Fact]
    public void CreateActivityEvent_WithNullTitle_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => new ActivityEvent(Guid.NewGuid(), null!, DateTime.UtcNow, "GitHub"));
    }
}
