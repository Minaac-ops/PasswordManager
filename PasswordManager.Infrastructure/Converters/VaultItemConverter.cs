using PasswordManager.Core.Models;
using PasswordManager.Infrastructure.Schemas;

namespace PasswordManager.Infrastructure.Converters;

public class VaultItemConverter
{
    public ItemModel Convert(VaultItem model)
    {
        return new ItemModel()
        {
            ItemName = model.ItemName,
            Username = model.Username,
            EncryptedPassword = model.EncryptedPassword,
            UserId = model.UserId
        };
    }
}