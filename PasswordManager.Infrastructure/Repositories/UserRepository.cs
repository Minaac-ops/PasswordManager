using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using PasswordManager.Core.Interfaces;
using PasswordManager.Core.Interfaces.Repositories;
using PasswordManager.Core.Models;
using PasswordManager.Infrastructure.Converters;
using PasswordManager.Infrastructure.Schemas;
using Exception = System.Exception;

namespace PasswordManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<SecureUserKeys> _userKeys;
    private readonly UserConverter _converter;
    private readonly IConfigurationRoot _config;

    public UserRepository()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var configPath = Path.Combine(basePath, "../../../../PasswordManager.Infrastructure/appsettings.json");

        _config = new ConfigurationBuilder()
            .AddJsonFile(configPath)
            .Build();
        
        _converter = new UserConverter();
        var settings = MongoClientSettings.FromConnectionString(_config.GetConnectionString("MongoDB"));

        var client = new MongoClient(settings);
        var passwordManagerDb = client.GetDatabase("PasswordManager");
        var encryptionParametersDb = client.GetDatabase("EncryptionParameters");
        
        _users = passwordManagerDb.GetCollection<User>("Users");
        _userKeys = encryptionParametersDb.GetCollection<SecureUserKeys>("SecureUserKeys");
    }
    
    public IEnumerable<UserModel> GetAllUsers()
    {
        try
        {
            var users = _users.Find(_ => true).ToList();
            var secureKeys = _userKeys.Find(_ => true).ToList();

            return _converter.Convert(users, secureKeys);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void CreateUser(UserModel user)
    {
        try
        {
            var insertOne = new User
            {
                Username = user.Username,
                EncryptedRandom = user.EncryptedRandom,
                Id = ObjectId.GenerateNewId()
            };
            _users.InsertOne(insertOne);
            _userKeys.InsertOne(new SecureUserKeys
            {
                UserId = insertOne.Id.ToString()!,
                IV = user.IV,
                PasswordSalt = user.PasswordSalt
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}