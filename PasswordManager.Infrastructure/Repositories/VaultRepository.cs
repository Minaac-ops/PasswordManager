using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
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
    private readonly IMongoCollection<SecureItemKeys> _itemKeys;
    private readonly VaultItemConverter _converter;
    private readonly IConfigurationRoot _config;

    public VaultRepository()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var configPath = Path.Combine(basePath, "../../../../PasswordManager.Infrastructure/appsettings.json");
        
        _config = new ConfigurationBuilder()
            .AddJsonFile(configPath)
            .Build();

        
        _converter = new VaultItemConverter();
       
        var settings = MongoClientSettings.FromConnectionString(_config.GetConnectionString("MongoDB"));
        var client = new MongoClient(settings);
        var passwordManagerDb = client.GetDatabase("PasswordManager");
        var encryptionParametersDb = client.GetDatabase("EncryptionParameters");

        _vaultItems = passwordManagerDb.GetCollection<VaultItem>("VaultItems");
        _itemKeys = encryptionParametersDb.GetCollection<SecureItemKeys>("SecureItemKeys");
    }

    public void SaveItem(ItemModel newItem)
    {
        var insertOne = new VaultItem
        {
            ItemName = newItem.ItemName,
            Username = newItem.Username,
            EncryptedPassword = newItem.EncryptedPassword,
            UserId = newItem.UserId,
            Id = ObjectId.GenerateNewId()
        };
        _vaultItems.InsertOne(insertOne);
        _itemKeys.InsertOne(new SecureItemKeys
        {
            ItemId = insertOne.Id.ToString()!,
            UserId = newItem.UserId,
            IV = newItem.IV
        });
    }

    public IEnumerable<ItemModel> GetItemsByUserId(string userId)
    {
        try
        {
            var vaultFilter = Builders<VaultItem>.Filter.Eq(v => v.UserId, userId);
            var secureKeyFilter = Builders<SecureItemKeys>.Filter.Eq(k => k.UserId, userId);
            
            var items = _vaultItems.Find(vaultFilter).ToList();
            var secureKeys = _itemKeys.Find(secureKeyFilter).ToList();
            
            return _converter.Convert(items, secureKeys);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}