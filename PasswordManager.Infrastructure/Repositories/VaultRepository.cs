using MongoDB.Driver;
using PasswordManager.Core.Interfaces;
using PasswordManager.Core.Interfaces.Repositories;
using PasswordManager.Core.Models;
using PasswordManager.Infrastructure.Converters;
using PasswordManager.Infrastructure.Schemas;

namespace PasswordManager.Infrastructure.Repositories;

public class VaultRepository : IVaultRepository
{
    private readonly IMongoCollection<VaultItem> _vaultItems;
    private readonly VaultItemConverter _converter;
    
    public VaultRepository()
    {
        _converter = new VaultItemConverter();
        const string connectionuri =
            "mongodb+srv://annechristensen:BVu4uofJTGhw1J9u@cluster0.eljsoo0.mongodb.net/?retryWrites=true&w=majority";

        var settings = MongoClientSettings.FromConnectionString(connectionuri);
        var client = new MongoClient(settings);
        var db = client.GetDatabase("PasswordManager");

        _vaultItems = db.GetCollection<VaultItem>("VaultItems");
    }

    public void SaveItem(ItemModel newItem)
    {
        Console.WriteLine(newItem.ItemName, newItem.EncryptedPassword, newItem.Username, newItem.UserId);
        Thread.Sleep(5);
        _vaultItems.InsertOne(new VaultItem
        {
            ItemName = newItem.ItemName,
            Username = newItem.Username,
            EncryptedPassword = newItem.EncryptedPassword,
            UserId = newItem.UserId
        });
    }

    public IEnumerable<ItemModel> GetItemsByUserId(string userId)
    {
        try
        {
            var filter = Builders<VaultItem>.Filter.Eq(v => v.UserId, userId);
            var items = _vaultItems.Find(filter).ToList();
            return items.Select(vault => _converter.Convert(vault));

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}