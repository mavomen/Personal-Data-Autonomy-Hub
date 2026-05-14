using PDH.Shared.Kernel;
using Xunit;

namespace PDH.DomainTests;

public class ValueObjectTests
{
    private class TestValueObject : ValueObject
    {
        public string Prop1 { get; }
        public int Prop2 { get; }
        public TestValueObject(string prop1, int prop2) { Prop1 = prop1; Prop2 = prop2; }
        protected override IEnumerable<object> GetEqualityComponents() { yield return Prop1; yield return Prop2; }
    }

    [Fact]
    public void Equal_When_All_Properties_Match()
    {
        var a = new TestValueObject("hi", 1);
        var b = new TestValueObject("hi", 1);
        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void Not_Equal_When_Property_Differs()
    {
        var a = new TestValueObject("hi", 1);
        var b = new TestValueObject("hi", 2);
        Assert.NotEqual(a, b);
        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void Not_Equal_When_Null()
    {
        var a = new TestValueObject("hi", 1);
        Assert.False(a.Equals(null));
        Assert.False(a == null);
        Assert.True(a != null);
    }

    [Fact]
    public void HashCode_Is_Consistent()
    {
        var a = new TestValueObject("hi", 1);
        var b = new TestValueObject("hi", 1);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }
}
