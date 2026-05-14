using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PDH.Api.Controllers;
using PDH.Application.Commands.ConnectIntegration;
using Xunit;

namespace PDH.ApplicationTests.Controllers;

public class IntegrationsControllerTests
{
    [Fact]
    public async Task Connect_Returns_NoContent()
    {
        var mediatorMock = new Mock<IMediator>();
        var controller = new IntegrationsController(mediatorMock.Object);

        var result = await controller.Connect("GitHub", new ConnectIntegrationRequest("token", "refresh", DateTime.UtcNow));

        Assert.IsType<NoContentResult>(result);
    }
}
