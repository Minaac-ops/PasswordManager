using PasswordManager.Core.Models;

namespace PasswordManager.Core.Interfaces.Services;

public interface IAuthService
{
    UserModel PasswordHasher(string username, string password);
    bool AuthenticateLogin(UserModel user, string providedPassword);
    string GetDecryptedPassword(string key, byte[] encryptedPassword);
    byte[] EncryptItemPassword(string key, string plaintextpass);
}