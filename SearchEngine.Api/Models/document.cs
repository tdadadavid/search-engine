using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SearchEngine.Models
{
    public class Document
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("url")]
    public string url { get; set; }

    [BsonElement("isIndexed")]
    public bool isIndexed { get; set; }

}
}
