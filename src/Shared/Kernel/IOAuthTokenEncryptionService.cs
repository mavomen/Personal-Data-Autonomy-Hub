namespace PDH.Shared.Kernel.Interfaces;

public interface IOAuthTokenEncryptionService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}
