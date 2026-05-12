using Moq;
using PDH.Application.Commands.RegisterUser;
using PDH.Shared.Infrastructure.Persistence;
using PDH.Shared.Infrastructure.Repositories;
using PDH.Shared.Infrastructure.DomainEvents;
using PDH.Modules.Identity;
using Moq;
using Xunit;

namespace PDH.ApplicationTests.Commands;

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_User_And_Return_Id()
    {
        // Arrange
        var dbContext = TestDbContextFactory.Create();
        var repo = new Repository<User>(dbContext);
        var uow = new UnitOfWork(dbContext, Mock.Of<IDomainEventDispatcher>());
        var handler = new RegisterUserCommandHandler(repo, uow);

        var command = new RegisterUserCommand("test@example.com", "Password123!");

        // Act
        var userId = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, userId);
        var user = await dbContext.Users.FindAsync(userId);
        Assert.NotNull(user);
        Assert.Equal("test@example.com", user.Email);
        Assert.True(BCrypt.Net.BCrypt.Verify("Password123!", user.PasswordHash));
    }
}
