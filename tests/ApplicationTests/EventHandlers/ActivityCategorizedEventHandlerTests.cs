using Microsoft.AspNetCore.SignalR;
using Moq;
using PDH.Api.EventHandlers;
using PDH.Api.Hubs;
using PDH.Shared.Kernel;
using Xunit;

namespace PDH.ApplicationTests.EventHandlers;

public class ActivityCategorizedEventHandlerTests
{
    [Fact]
    public async Task Handle_Sends_Message_To_Hub()
    {
        var hubContextMock = new Mock<IHubContext<NotificationHub>>();
        var clientsMock = new Mock<IHubClients>();
        var clientProxyMock = new Mock<IClientProxy>();
        hubContextMock.Setup(h => h.Clients).Returns(clientsMock.Object);
        clientsMock.Setup(c => c.All).Returns(clientProxyMock.Object);

        var handler = new ActivityCategorizedEventHandler(hubContextMock.Object);
        var evt = new ActivityCategorizedEvent(Guid.NewGuid(), "DeepWork");

        await handler.Handle(evt, CancellationToken.None);

        clientProxyMock.Verify(c => c.SendCoreAsync("ActivityCategorized", It.Is<object[]>(o => o.Length > 0), It.IsAny<CancellationToken>()), Times.Once);
    }
}
