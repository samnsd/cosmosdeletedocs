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
    [BsonElement("version")]
    public int Version { get; set; }
    [BsonElement("metadataId")]
    public string MetadataId { get; set; } = string.Empty;
    [BsonElement("parentTaxonId")]
    public string ParentTaxonId { get; set; } = string.Empty;
    [BsonElement("sortOrder")]
    public int SortOrder { get; set; }
    [BsonElement("langVersions")]   
    public List<TaxonMetadataLangVersion> LangVersions { get; set; } = [];
    [BsonElement("earlierVersionIds")]
    public List<string> EarlierVersionIds { get; set; } = [];
}

public class TaxonMetadataLangVersion
{
    [BsonElement("langCode")]
    public string LangCode { get; set; } = string.Empty;
    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;
    [BsonElement("altTitle")]
    public string? AltTitle { get; set; }
    [BsonElement("description")]
    public string? Description { get; set; }
}