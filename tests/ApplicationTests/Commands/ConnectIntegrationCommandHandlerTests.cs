using PDH.Application.Commands.ConnectIntegration;
using PDH.Application.Interfaces;
using PDH.Modules.Identity;
using PDH.Shared.Infrastructure.Persistence;
using PDH.Shared.Infrastructure.Repositories;
using PDH.Shared.Kernel.Interfaces;
using Moq;
using Xunit;

namespace PDH.ApplicationTests.Commands;

public class ConnectIntegrationCommandHandlerTests
{
    [Fact]
    public async Task Handle_Adds_OAuth_Integration()
    {
        var userId = Guid.NewGuid();
        var user = new User(userId, "a@b.com", "hash");
        var repoMock = new Mock<IRepository<User>>();
        repoMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        var uowMock = new Mock<IUnitOfWork>();
        var currentUserMock = new Mock<ICurrentUserService>();
        currentUserMock.Setup(c => c.UserId).Returns(userId);
        var encryptionMock = new Mock<IOAuthTokenEncryptionService>();
        encryptionMock.Setup(e => e.Encrypt(It.IsAny<string>())).Returns((string s) => s + "_encrypted");

        var handler = new ConnectIntegrationCommandHandler(repoMock.Object, uowMock.Object, currentUserMock.Object, encryptionMock.Object);
        var command = new ConnectIntegrationCommand("GitHub", "access", "refresh", DateTime.UtcNow.AddHours(1));

        await handler.Handle(command, CancellationToken.None);

        Assert.Single(user.OAuthIntegrations);
        Assert.Equal("GitHub", user.OAuthIntegrations.First().Provider);
        Assert.Equal("access_encrypted", user.OAuthIntegrations.First().EncryptedAccessToken);
        Assert.Equal("refresh_encrypted", user.OAuthIntegrations.First().EncryptedRefreshToken);
        repoMock.Verify(r => r.Update(user), Times.Once);
        uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
