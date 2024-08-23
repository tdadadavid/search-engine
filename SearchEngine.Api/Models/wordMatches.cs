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
    
    [BsonElement("link")]
    public string DocLink { get; set; }

    [BsonElement("positions")]
    public List<int> Positions { get; set; } = new List<int>();
    } 

    public class MatchResult
    {
      [BsonElement("docId")]
      public string DocId { get; set; }

      [BsonElement("rank")]
      public double Rank { get; set; }

      [BsonElement("link")]
      public string Link { get; set; }
  }
}