using PDH.Shared.Kernel;
using Xunit;

namespace PDH.DomainTests;

public class StronglyTypedIdNullTests
{
    private record NullableTestId(Guid? Value) : StronglyTypedId<Guid?>(Value);

    [Fact]
    public void ToString_WhenValueIsNull_Returns_Empty_String()
    {
        var id = new NullableTestId(null);
        Assert.Equal("NullableTestId { Value =  }", id.ToString());
    }
}
