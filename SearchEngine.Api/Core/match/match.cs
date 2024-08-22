using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace SearchEngine.Api.Match
{
    class DocResult
    {
        /// <summary>
        /// Ranks the list of matches and returns a MatchList sorted in Descending Order (Largest comes first)
        /// </summary>
        /// <param name="res">MAtches to be ranked</param>
        /// <returns>Ranked result</returns>
        public List<MatchList> Rank(List<MatchList> res)
        {
            return res.OrderByDescending(item => (item.freq * 0.7) + (item.proxScore * 0.3)).ToList();
        }

        /// <summary>
        /// Parses all words and matches into a single easy to rank form
        /// </summary>
        /// <param name="res">results to be parsed</param>
        /// <returns>Final ranked aggregate</returns>
        public List<MatchList> RankAll(List<DocMatch> res)
        {
            List<MatchList> aggregate = res.SelectMany(items => items.matches)
                                            .GroupBy(group => group.id)
                                            .Select(item => new MatchList
                                            {
                                                id = item.Key,
                                                pos = item.SelectMany(subItem => subItem.pos).ToList(),
                                                freq = item.Sum(subItem => subItem.pos.Count),
                                                proxScore = CalcProximityScore(item.SelectMany(subItem => subItem.pos).ToList())
                                            })
                                            .ToList();
            return Rank(aggregate);
        }

        /// <summary>
        /// Gets the proximity score of words in a Doc
        /// </summary>
        /// <param name="positions">List of positions in Doc</param>
        /// <returns>Proximity score as a double</returns
        private static double CalcProximityScore(List<int> positions)
        {
            if (positions.Count <= 1)
            {
                return 0; // No proximity info here
            }

            positions.Sort();
            double score = 0;
            for (int i = 1; i < positions.Count; i++)
            {
                score += positions[i] - positions[i - 1];
            }
            double avgScore = score / (positions.Count - 1);

            // The smaller the average score, the higher the final proximity score
            return 1 / avgScore;
        }
    }

    public class DocMatch
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("word")]
        public string word { get; set; }

        [BsonElement("matches")]
        public List<MatchList> matches { get; set; }
    }

    public class MatchList
    {
        [BsonElement("docId")]
        public string id { get; set; }

        [BsonElement("positions")]
        public List<int> pos { get; set; }

        public int freq { get; set; }

        public double proxScore;
    }
}
