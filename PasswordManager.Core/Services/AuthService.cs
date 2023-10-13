using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using PasswordManager.Core.Interfaces;
using PasswordManager.Core.Interfaces.Services;
using PasswordManager.Core.Models;
using PasswordManager.Infrastructure.Schemas;

namespace PasswordManager.Core.Services;

public class AuthService : IAuthService
{
    public bool AuthenticateLogin(UserModel user, string providedPassword)
    {
        using var df2 = new Rfc2898DeriveBytes(providedPassword, user.PasswordSalt, 10000, HashAlgorithmName.SHA512);
        var calculatedHash = df2.GetBytes(64);

        return CompareByteArrays(calculatedHash, user.PasswordHash);
    }

    public string GetDecryptedPassword(string key, byte[] encryptedPassword)
    {
        using var aes = Aes.Create();
        
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = new byte[16];

        var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream(encryptedPassword);
        using var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    public byte[] EncryptItemPassword(string key, string plaintextpass)
    {
        byte[] encryptedPass;

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plaintextpass);
                    }
                    encryptedPass = ms.ToArray();
                }
            }
        }

        return encryptedPass;
    }

    private bool CompareByteArrays(byte[] calculatedHash, byte[] userPasswordHash)
    {
        if (calculatedHash.Length != userPasswordHash.Length)
        {
            return false;
        }

        return !calculatedHash.Where((t, i) => t != userPasswordHash[i]).Any();
    }

    public UserModel PasswordHasher(string username, string password)
    {
        GenerateUserCredentials(password, out var passwordHash, out var passwordSalt);

        var user = new UserModel
        {
            Username = username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        return user;
    }

    private void GenerateUserCredentials(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var rng = new RNGCryptoServiceProvider();
        // generating a random salt
        var salt = new byte[32];
        rng.GetBytes(salt);

        // use Rfc with 10.000 iterations og sha for password hashing.
        using var df2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA512);
        
        passwordHash = df2.GetBytes(64);
        passwordSalt = salt;
    }
}