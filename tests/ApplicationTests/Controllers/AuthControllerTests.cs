using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PDH.Api.Controllers;
using PDH.Application.Commands.RegisterUser;
using PDH.Application.Queries.Login;
using Xunit;

namespace PDH.ApplicationTests.Controllers;

public class AuthControllerTests
{
    [Fact]
    public async Task Register_Returns_Created()
    {
        var mediatorMock = new Mock<IMediator>();
        var userId = Guid.NewGuid();
        mediatorMock.Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(userId);
        var controller = new AuthController(mediatorMock.Object);

        var result = await controller.Register(new RegisterUserCommand("a@b.com", "Password123!"));

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.NotNull(createdResult.Value); // the returned object is non‑null
    }

    [Fact]
    public async Task Login_Valid_Returns_Token()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<LoginQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync("test-jwt");
        var controller = new AuthController(mediatorMock.Object);

        var result = await controller.Login(new LoginQuery("a@b.com", "pass"));

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Login_Invalid_Returns_Unauthorized()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<LoginQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((string?)null);
        var controller = new AuthController(mediatorMock.Object);

        var result = await controller.Login(new LoginQuery("x", "y"));

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}
