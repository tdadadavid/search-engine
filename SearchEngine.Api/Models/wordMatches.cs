using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SearchEngine.Models
{
    public class Match
    {
    [BsonElement("docId")]
    public string DocId { get; set; }

    [BsonElement("positions")]
    public List<int> Positions { get; set; } = new List<int>();
    }
}