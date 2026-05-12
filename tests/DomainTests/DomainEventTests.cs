using PDH.Modules.Identity;
using PDH.Shared.Kernel;
using Xunit;

namespace PDH.DomainTests;

public class DomainEventTests
{
    [Fact]
    public void User_Clears_DomainEvents_When_Called()
    {
        var user = new User(Guid.NewGuid(), "a@b.com", "hash");
        user.RecordLogin();
        user.ClearDomainEvents();
        Assert.Empty(user.DomainEvents);
    }

    [Fact]
    public void DomainEvent_Has_OccurredOn()
    {
        var evt = new ActivityImportedEvent(Guid.NewGuid());
        Assert.True(evt.OccurredOn <= DateTime.UtcNow);
    }
}
