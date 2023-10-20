using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PasswordManager.Infrastructure.Schemas;

public class SecureItemKeys
{
    [BsonId] public ObjectId Id { get; set; }
    [BsonElement("ItemId")] public string ItemId { get; set; }
    [BsonElement("UserId")] public string UserId { get; set; }
    [BsonElement("IV")] public byte[] IV { get; set; }
}