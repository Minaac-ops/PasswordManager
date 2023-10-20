using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PasswordManager.Infrastructure.Schemas;

public class SecureUserKeys
{
    [BsonId] public ObjectId Id { get; set; }
    [BsonElement("UserId")] public string UserId { get; set; }
    [BsonElement("IV")] public byte[] IV { get; set; }
    [BsonElement("PasswordSalt")] public byte[] PasswordSalt { get; set; }
}