using System.Collections.Immutable;
using MongoDB.Driver;
using PasswordManager.Core.Interfaces;
using PasswordManager.Core.Interfaces.Repositories;
using PasswordManager.Core.Models;
using PasswordManager.Infrastructure.Converters;
using PasswordManager.Infrastructure.Schemas;

namespace PasswordManager.Infrastructure.Repositories;

public class PasswordRepository : IPasswordRepository
{
    private readonly IMongoCollection<User> _users;
    private readonly UserConverter _converter;
    
    public PasswordRepository()
    {
        _converter = new UserConverter();
        const string connectionuri =
            "mongodb+srv://annechristensen:BVu4uofJTGhw1J9u@cluster0.eljsoo0.mongodb.net/?retryWrites=true&w=majority";

        var settings = MongoClientSettings.FromConnectionString(connectionuri);
        var client = new MongoClient(settings);
        var db = client.GetDatabase("PasswordManager");

        _users = db.GetCollection<User>("Users");
    }
}