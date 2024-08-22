using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SearchEngine.Models
{
    public class Document
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ID { get; set; }

    [BsonElement("url")]
    public required string Url { get; set; }

    [BsonElement("content")]

    public List<string> Content { get; set; } = new List<string>();

    [BsonElement("isIndexed")]
    public bool IsIndexed { get; set; }

    [BsonElement("type")] // pptx, docx, doc.
    public string Type { get; set; }

}
}
