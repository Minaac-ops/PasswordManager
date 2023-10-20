using System.Net.Security;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;
using PasswordManager.Core.Interfaces;
using PasswordManager.Core.Interfaces.Services;
using PasswordManager.Core.Models;
using PasswordManager.Infrastructure.Schemas;

namespace PasswordManager.Core.Services;

public class AuthService : IAuthService
{
    private byte[] key;
    private readonly IVaultService _vaultService;

    public AuthService(IVaultService vaultService)
    {
        _vaultService = vaultService;
    }
    
    public bool AuthenticateLogin(UserModel user, string providedPassword)
    {
        using var df2 = new Rfc2898DeriveBytes(providedPassword, user.PasswordSalt, 600000, HashAlgorithmName.SHA512);
        var calculatedHash = df2.GetBytes(256/8);
        Console.WriteLine(user.EncryptedRandom);
        var tryToDecrypt = user.EncryptedRandom;

        using var aes = Aes.Create();

        aes.Key = calculatedHash;
        aes.IV = user.IV;

        try
        {
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
        
            using var ms = new MemoryStream(tryToDecrypt);
            using var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            key = calculatedHash;
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid username or password");
            return false;
        }
    }

    public string GetDecryptedPassword(ItemModel item)
    {
        using var aes = Aes.Create();
        
        aes.Key = key;
        aes.IV = item.IV;

        var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream(item.EncryptedPassword);
        using var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    public void EncryptItemPassword(ItemModel newItem, string plaintextpass)
    {
        byte[] encryptedPass;

        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.GenerateIV();

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
            newItem.EncryptedPassword = encryptedPass;
            newItem.IV = aes.IV;
        }
        _vaultService.SaveItem(newItem);
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
        GenerateUserCredentials(password, out var iv, out var passwordSalt, out var random);
        
        var user = new UserModel
        {
            Username = username,
            PasswordSalt = passwordSalt,
            IV = iv,
            EncryptedRandom = random
        };
        return user;
    }

    private void GenerateUserCredentials(string password, out byte[] iv, out byte[] passwordSalt, out byte[] random)
    {
        using var aes = Aes.Create();
        aes.GenerateIV();
        
        using var rng = new RNGCryptoServiceProvider();
        // generating a random salt
        var salt = new byte[32];
        rng.GetBytes(salt);

        // use Rfc with 60.000 iterations og sha for password hashing.
        using var df2 = new Rfc2898DeriveBytes(password, salt, 600000, HashAlgorithmName.SHA512);
        aes.Key = df2.GetBytes(256/8);
        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(GenerateRandomString());
                }
                random = ms.ToArray();
            }
        }
        iv = aes.IV;
        passwordSalt = salt;
    }

    private string GenerateRandomString()
    {
        var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var sb = new StringBuilder(12);
        for (int i = 0; i < 12; i++)
        {
            int index = random.Next(chars.Length);
            sb.Append(chars[index]);
        }
        return sb.ToString();
    }
}