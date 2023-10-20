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

    public IEnumerable<ItemModel> Convert(List<VaultItem> items, List<SecureItemKeys> keys)
    {
        var combinedItems = items.Select(vault => new ItemModel
        {
            ItemName = vault.ItemName,
            Username = vault.Username,
            EncryptedPassword = vault.EncryptedPassword,
            UserId = vault.UserId,
            IV = keys.FirstOrDefault(key => key.ItemId == vault.Id.ToString())!.IV
        });
        return combinedItems;
    }
}