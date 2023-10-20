using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PasswordManager.Infrastructure.Schemas;


public class User
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    [BsonElement("Username")]
    public string Username { get; set; }
    [BsonElement("EncryptedRandom")]
    public byte[] EncryptedRandom { get; set; }
}