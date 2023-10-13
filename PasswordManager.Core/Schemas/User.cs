using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PasswordManager.Infrastructure.Schemas;


public class User
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    [BsonElement("Username")]
    public string Username { get; set; }
    
    [BsonElement("HashedPassword")]
    public byte[] PasswordHash { get; set; }

    [BsonElement("PasswordSalt")]
    public byte[] PasswordSalt { get; set; }
}