using Moq;
using PDH.Application.Queries.Login;
using PDH.Modules.Identity;
using PDH.Shared.Kernel.Interfaces;

namespace PDH.ApplicationTests.Queries;

public class LoginQueryHandlerTests
{
    [Fact]
    public async Task Handle_With_Valid_Credentials_Returns_Token()
    {
        var dbContext = TestDbContextFactory.Create();
        var tokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        tokenGeneratorMock.Setup(t => t.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>()))
                          .Returns("test-jwt");

        var user = new User(Guid.NewGuid(), "test@example.com", BCrypt.Net.BCrypt.HashPassword("password"));
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var handler = new LoginQueryHandler(dbContext, tokenGeneratorMock.Object);
        var query = new LoginQuery("test@example.com", "password");

        var token = await handler.Handle(query, CancellationToken.None);

        Assert.Equal("test-jwt", token);
        Assert.NotNull(user.LastLoginAt);
    }

    [Fact]
    public async Task Handle_With_Invalid_Credentials_Returns_Null()
    {
        var dbContext = TestDbContextFactory.Create();
        var tokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        var handler = new LoginQueryHandler(dbContext, tokenGeneratorMock.Object);
        var query = new LoginQuery("unknown@example.com", "password");

        var token = await handler.Handle(query, CancellationToken.None);

        Assert.Null(token);
    }
}
