using Microsoft.AspNetCore.DataProtection;
using PDH.Shared.Infrastructure.Auth;
using Xunit;

namespace PDH.ApplicationTests.Auth;

public class OAuthTokenEncryptionServiceTests
{
    [Fact]
    public void Encrypt_Decrypt_Roundtrip()
    {
        var provider = new EphemeralDataProtectionProvider();
        var service = new OAuthTokenEncryptionService(provider);
        var plainText = "my-secret-token";
        var encrypted = service.Encrypt(plainText);
        var decrypted = service.Decrypt(encrypted);
        Assert.Equal(plainText, decrypted);
        Assert.NotEqual(plainText, encrypted);
    }
}
