using MediatR;
using PDH.Application.Behaviors;
using Xunit;

namespace PDH.ApplicationTests.Behaviors;

public class LoggingBehaviorTests
{
    [Fact]
    public async Task Handle_Logs_And_Returns_Response()
    {
        var logger = new TestLogger<LoggingBehavior<TestRequest, bool>>();
        var behavior = new LoggingBehavior<TestRequest, bool>(logger);
        var result = await behavior.Handle(new TestRequest { Name = "log" }, () => Task.FromResult(true), CancellationToken.None);
        Assert.True(result);
        Assert.True(logger.CallCount >= 2, $"Expected at least 2 log calls, got {logger.CallCount}");
    }
}
