using PDH.Shared.Kernel;

namespace PDH.Modules.Identity;

public class OAuthIntegration : Entity
{
    public string Provider { get; private set; }
    public string EncryptedAccessToken { get; private set; }
    public string EncryptedRefreshToken { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public Guid UserId { get; private set; }   // FK to User

    private OAuthIntegration()
    {
        Provider = null!;
        EncryptedAccessToken = null!;
        EncryptedRefreshToken = null!;
    }

    public OAuthIntegration(string provider, string accessToken, string refreshToken, DateTime expiresAt, Guid userId)
    {
        Id = Guid.NewGuid();
        Provider = provider ?? throw new DomainException("Provider is required");
        EncryptedAccessToken = accessToken;
        EncryptedRefreshToken = refreshToken;
        ExpiresAt = expiresAt;
        UserId = userId;
    }
}
