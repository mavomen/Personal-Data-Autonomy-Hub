using FluentValidation;
using MediatR;
using PDH.Application.Behaviors;
using Xunit;

namespace PDH.ApplicationTests.Behaviors;

public class ValidationBehaviorTests
{
    private class TestRequest : IRequest<bool> { public string Name { get; set; } = string.Empty; }
    private class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator() { RuleFor(x => x.Name).NotEmpty(); }
    }

    [Fact]
    public async Task Handle_Valid_Request_Passes()
    {
        var validators = new[] { new TestRequestValidator() };
        var behavior = new ValidationBehavior<TestRequest, bool>(validators);
        var result = await behavior.Handle(new TestRequest { Name = "valid" }, () => Task.FromResult(true), CancellationToken.None);
        Assert.True(result);
    }

    [Fact]
    public async Task Handle_Invalid_Request_Throws()
    {
        var validators = new[] { new TestRequestValidator() };
        var behavior = new ValidationBehavior<TestRequest, bool>(validators);
        await Assert.ThrowsAsync<ValidationException>(() =>
            behavior.Handle(new TestRequest { Name = "" }, () => Task.FromResult(true), CancellationToken.None));
    }
}
