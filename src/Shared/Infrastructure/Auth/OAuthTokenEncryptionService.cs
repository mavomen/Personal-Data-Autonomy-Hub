using Microsoft.AspNetCore.DataProtection;
using PDH.Shared.Kernel.Interfaces;

namespace PDH.Shared.Infrastructure.Auth;

public class OAuthTokenEncryptionService : IOAuthTokenEncryptionService
{
    private readonly IDataProtector _protector;

    public OAuthTokenEncryptionService(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("OAuthTokens");
    }

    public string Encrypt(string plainText)
    {
        return _protector.Protect(plainText);
    }

    public string Decrypt(string cipherText)
    {
        return _protector.Unprotect(cipherText);
    }
}
