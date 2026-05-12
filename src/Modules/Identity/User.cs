using PDH.Shared.Kernel;

namespace PDH.Modules.Identity;

public class User : Entity, IAggregateRoot
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private readonly List<OAuthIntegration> _oauthIntegrations = new();
    public IReadOnlyCollection<OAuthIntegration> OAuthIntegrations => _oauthIntegrations.AsReadOnly();

    private User()
    {
        Email = null!;
        PasswordHash = null!;
    }

    public User(Guid id, string email, string passwordHash)
    {
        Id = id;
        Email = email ?? throw new DomainException("Email is required");
        PasswordHash = passwordHash ?? throw new DomainException("Password hash is required");
        CreatedAt = DateTime.UtcNow;
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public OAuthIntegration AddOAuthIntegration(string provider, string accessToken, string refreshToken, DateTime expiresAt)
    {
        var integration = new OAuthIntegration(provider, accessToken, refreshToken, expiresAt, Id);
        _oauthIntegrations.Add(integration);
        return integration;
    }
}
