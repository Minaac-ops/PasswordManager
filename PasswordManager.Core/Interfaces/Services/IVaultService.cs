using PasswordManager.Core.Models;

namespace PasswordManager.Core.Interfaces.Services;

public interface IVaultService
{
    void SaveItem(ItemModel newItem);
    IEnumerable<ItemModel> GetItemsByUserId(string userId);
}