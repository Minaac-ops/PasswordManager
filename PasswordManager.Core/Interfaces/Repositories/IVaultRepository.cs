using PasswordManager.Core.Models;
using PasswordManager.Infrastructure.Schemas;

namespace PasswordManager.Core.Interfaces.Repositories;

public interface IVaultRepository
{
    void SaveItem(ItemModel newItem);
    IEnumerable<ItemModel> GetItemsByUserId(string userId);
}