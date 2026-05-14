using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PDH.Shared.Infrastructure.DomainEvents;
using PDH.Shared.Kernel;
using Xunit;

namespace PDH.ApplicationTests.DomainEvents;

public class DomainEventDispatcherTests
{
    [Fact]
    public async Task Dispatch_Invokes_Mediator_For_Each_Event()
    {
        var mediatorMock = new Mock<IMediator>();
        var serviceProvider = new Mock<IServiceProvider>();
        var scope = new Mock<IServiceScope>();
        var scopeFactory = new Mock<IServiceScopeFactory>();

        serviceProvider.Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
                       .Returns(scopeFactory.Object);
        scopeFactory.Setup(s => s.CreateScope()).Returns(scope.Object);
        scope.Setup(s => s.ServiceProvider).Returns(serviceProvider.Object);
        serviceProvider.Setup(sp => sp.GetService(typeof(IMediator))).Returns(mediatorMock.Object);

        var dispatcher = new DomainEventDispatcher(serviceProvider.Object);
        var domainEvents = new List<IDomainEvent>
        {
            new ActivityImportedEvent(Guid.NewGuid()),
            new ActivityImportedEvent(Guid.NewGuid())
        };

        await dispatcher.DispatchAsync(domainEvents, CancellationToken.None);

        mediatorMock.Verify(
            m => m.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }
}
