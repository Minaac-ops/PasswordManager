using PasswordManager.Core.Models;
using PasswordManager.Infrastructure.Schemas;

namespace PasswordManager.Infrastructure.Converters;

public class UserConverter
{
    public User Convert(UserModel model)
    {
        return new User
        {
            Username = model.Username,
            PasswordHash = model.PasswordHash,
            PasswordSalt = model.PasswordSalt
        };
    }

    public UserModel Convert(User schema)
    {
        return new UserModel
        {
            Id = schema.Id.ToString(),
            Username = schema.Username,
            PasswordHash = schema.PasswordHash,
            PasswordSalt = schema.PasswordSalt
        };
    }
}