using MongoDB.Driver;
using PasswordManager.Core.Interfaces.Repositories;
using PasswordManager.Infrastructure.Converters;
using PasswordManager.Infrastructure.Schemas;
using Microsoft.Extensions.Configuration;

namespace PasswordManager.Infrastructure.Repositories;

public class PasswordRepository : IPasswordRepository
{
    private readonly IMongoCollection<User> _users;
    private readonly UserConverter _converter;
    private readonly IConfiguration _config;
    
    public PasswordRepository()
    {
        _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        _converter = new UserConverter();

        var settings = MongoClientSettings.FromConnectionString(_config.GetConnectionString("MongoDB"));
        var client = new MongoClient(settings);
        var db = client.GetDatabase("PasswordManager");

        _users = db.GetCollection<User>("Users");
    }
}