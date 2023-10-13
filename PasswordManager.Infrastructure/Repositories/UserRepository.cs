using MongoDB.Driver;
using PasswordManager.Core.Interfaces;
using PasswordManager.Core.Interfaces.Repositories;
using PasswordManager.Core.Models;
using PasswordManager.Infrastructure.Converters;
using PasswordManager.Infrastructure.Schemas;

namespace PasswordManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;
    private readonly UserConverter _converter;
    
    public UserRepository()
    {
        _converter = new UserConverter();
        const string connectionuri =
            "mongodb+srv://annechristensen:BVu4uofJTGhw1J9u@cluster0.eljsoo0.mongodb.net/?retryWrites=true&w=majority";

        var settings = MongoClientSettings.FromConnectionString(connectionuri);
        var client = new MongoClient(settings);
        var db = client.GetDatabase("PasswordManager");

        _users = db.GetCollection<User>("Users");
    }
    
    public IEnumerable<UserModel> GetAllUsers()
    {
        try
        {
            var users = _users.Find(_ => true).ToList();
            return users.Select(user => _converter.Convert(user));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public UserModel GetUser(string email)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, email);
            var user = _users.FindAsync(filter).Result.FirstOrDefault();
            return _converter.Convert(user);
        }
        catch (Exception e)
        {
            Console.WriteLine("Please provide a valid email address and password.");
            throw;
        }
    }

    public void CreateUser(UserModel user)
    {
        try
        {
            _users.InsertOne(new User
            {
                Username = user.Username,
                PasswordHash = user.PasswordHash,
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