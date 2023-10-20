using PasswordManager.Core.Models;
using PasswordManager.Infrastructure.Schemas;

namespace PasswordManager.Infrastructure.Converters;

public class UserConverter
{
    public IEnumerable<UserModel> Convert(List<User> users, List<SecureUserKeys> keys)
    {
        var combinedUsers = users.Select(user => new UserModel
        {
            Id = user.Id.ToString()!,
            Username = user.Username,
            EncryptedRandom = user.EncryptedRandom,
            IV = keys.FirstOrDefault(k => k.UserId == user.Id.ToString())!.IV,
            PasswordSalt = keys.FirstOrDefault(k => k.UserId == user.Id.ToString())!.PasswordSalt
        });
        return combinedUsers;
    }
}