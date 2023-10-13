using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Text;
using PasswordManager.Core.Interfaces.Repositories;
using PasswordManager.Core.Interfaces.Services;
using PasswordManager.Core.Models;
using PasswordManager.Infrastructure.Schemas;

namespace PasswordManager.Core.Services;

public class VaultService : IVaultService
{
    private readonly IVaultRepository _repo;
    public VaultService(IVaultRepository repo)
    {
        _repo = repo;
    }
    public void SaveItem(ItemModel newItem)
    {
        _repo.SaveItem(newItem);
    }

    public IEnumerable<ItemModel> GetItemsByUserId(string userId)
    {
        return _repo.GetItemsByUserId(userId);
    }
}