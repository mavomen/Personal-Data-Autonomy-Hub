using PDH.Shared.Kernel;
using Xunit;

namespace PDH.DomainTests;

public class DomainExceptionTests
{
    [Fact]
    public void DomainException_WithInnerException_PreservesInner()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new DomainException("outer", inner);
        Assert.Equal("outer", ex.Message);
        Assert.Same(inner, ex.InnerException);
    }
}
