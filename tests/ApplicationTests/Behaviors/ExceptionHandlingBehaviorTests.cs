using MediatR;
using PDH.Application.Behaviors;
using Xunit;

namespace PDH.ApplicationTests.Behaviors;

public class ExceptionHandlingBehaviorTests
{
    [Fact]
    public async Task Handle_ReThrows_And_Logs()
    {
        var logger = new TestLogger<ExceptionHandlingBehavior<TestRequest, bool>>();
        var behavior = new ExceptionHandlingBehavior<TestRequest, bool>(logger);
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            behavior.Handle(new TestRequest { Name = "err" }, () => throw new InvalidOperationException(), CancellationToken.None));
        Assert.True(logger.CallCount >= 1, $"Expected at least 1 log call, got {logger.CallCount}");
    }
}
