using PDH.Shared.Kernel;
using Xunit;

namespace PDH.DomainTests;

public class StronglyTypedIdTests
{
    [Fact]
    public void UserId_Has_Value()
    {
        var id = new UserId(Guid.NewGuid());
        Assert.NotEqual(Guid.Empty, id.Value);
    }

    [Fact]
    public void ActivityId_Has_Value()
    {
        var id = new ActivityId(Guid.NewGuid());
        Assert.NotEqual(Guid.Empty, id.Value);
    }

    [Fact]
    public void ToString_Contains_Value()
    {
        var id = new UserId(Guid.Parse("12345678-1234-1234-1234-123456789abc"));
        Assert.Contains("12345678-1234-1234-1234-123456789abc", id.ToString());
    }
}
