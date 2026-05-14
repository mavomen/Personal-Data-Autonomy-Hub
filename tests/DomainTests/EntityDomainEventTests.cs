using PDH.Modules.Identity;
using PDH.Shared.Kernel;
using Xunit;

namespace PDH.DomainTests;

public class EntityDomainEventTests
{
    [Fact]
    public void ClearDomainEvents_RemovesAllEvents()
    {
        var user = new User(Guid.NewGuid(), "a@b.com", "hash");
        user.RecordLogin(); // this adds a domain event? Actually User doesn't add domain events in our implementation.
        // We'll test with a custom entity that adds domain events.
        var entity = new TestEntity();
        entity.AddTestEvent();
        Assert.NotEmpty(entity.DomainEvents);
        entity.ClearDomainEvents();
        Assert.Empty(entity.DomainEvents);
    }

    private class TestEntity : Entity
    {
        public void AddTestEvent() => AddDomainEvent(new ActivityImportedEvent(Guid.NewGuid()));
    }
}
