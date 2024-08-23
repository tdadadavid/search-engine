using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SearchEngine.Models
{
    /// <summary>
    /// Represents a match occurrence of a word in a document.
    /// </summary>
    public class Match
    {
    [BsonElement("docId")]
    public string DocId { get; set; }

    [BsonElement("positions")]
    public List<int> Positions { get; set; } = new List<int>();
    }
}