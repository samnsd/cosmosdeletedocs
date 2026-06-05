using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CosmosMongoDelete.Models;

public class CosmosDocument
{
    [BsonId]
    public string Id { get; set; } = default!;

    [BsonElement("sk")]
    public string SK { get; set; }

    [BsonElement("docType")]
    public string DocType { get; set; } = default!;
}
