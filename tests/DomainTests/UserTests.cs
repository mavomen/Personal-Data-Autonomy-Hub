using PDH.Shared.Kernel;
using PDH.Modules.Identity;
using Xunit;

namespace PDH.DomainTests;

public class UserTests
{
    [Fact]
    public void CreateUser_AssignsValidEmailAndHash()
    {
        var user = new User(Guid.NewGuid(), "test@example.com", "hashed");

        Assert.Equal("test@example.com", user.Email);
        Assert.Equal("hashed", user.PasswordHash);
    }

    [Fact]
    public void CreateUser_WithInvalidEmail_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => new User(Guid.NewGuid(), null!, "hash"));
    }

    [Fact]
    public void RecordLogin_SetsLastLoginAt()
    {
        var user = new User(Guid.NewGuid(), "a@b.com", "hash");
        user.RecordLogin();
        Assert.NotNull(user.LastLoginAt);
    }

    [Fact]
    public void AddOAuthIntegration_CreatesIntegrationLinkedToUser()
    {
        var user = new User(Guid.NewGuid(), "a@b.com", "hash");
        var integration = user.AddOAuthIntegration("GitHub", "token", "refresh", DateTime.UtcNow.AddHours(1));

        Assert.Equal("GitHub", integration.Provider);
        Assert.Equal(user.Id, integration.UserId);
    }
}
