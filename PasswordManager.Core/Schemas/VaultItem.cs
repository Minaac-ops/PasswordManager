using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PasswordManager.Infrastructure.Schemas;

public class VaultItem
{
    [BsonId] public ObjectId Id { get; set; }
    [BsonElement("ItemName")] public string ItemName { get; set; }
    [BsonElement("Username")] public string Username { get; set; }
    [BsonElement("EncryptedPassword")] public byte[] EncryptedPassword { get; set; }
    [BsonElement("UserId")] public string UserId { get; set; }
    
}