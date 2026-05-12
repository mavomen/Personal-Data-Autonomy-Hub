namespace PDH.Application.Interfaces;

public interface IOAuthTokenEncryptionService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}
