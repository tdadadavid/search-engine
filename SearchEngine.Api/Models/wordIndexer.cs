using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;



namespace SearchEngine.Models
{
    /// <summary>
    /// Represents an index entry for a word and its occurrences in documents.
    /// </summary>
    public class WordIndexer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("word")]
    public string Word { get; set; }

    [BsonElement("matches")]
    public List<Match> Matches { get; set; } = new List<Match>();
}

}



// public class WordMatch
// {
//     [BsonId]
//     [BsonRepresentation(BsonType.ObjectId)]
//     public string Id { get; set; }

//     [BsonElement("word")]
//     public string Word { get; set; }

//     [BsonElement("matches")]
//     public List<Match> Matches { get; set; }
// }
