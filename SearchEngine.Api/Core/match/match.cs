using System.Text.Json;
using System.Text.Json.Serialization;

namespace match
{
    class Match
    {
        //Use main function to test Matching
        public static void Main(string[] args)
        {
            string path = @"C:\Users\USER\Documents\example.json";
            string file = File.ReadAllText(path);
            List<DocMatch> myJson = JsonSerializer.Deserialize<List<DocMatch>>(file);
            Console.WriteLine(myJson[1].word);
            myJson[1].matches = new DocResult().Rank(myJson[1].matches);
            foreach (MatchList item in new DocResult().RankAll(myJson))
            {
                Console.WriteLine($"{item.id,15}: {item.freq}");
            }
        }
    }

    class DocResult
    {

        public List<MatchList> Rank(List<MatchList> res)
        {
            return res.OrderByDescending(item => (item.freq * 0.7) + (item.proxScore * 0.3)).ToList();
        }

        public List<MatchList> RankAll(List<DocMatch> res)
        {
            List<MatchList> aggregate = res.SelectMany(items => items.matches)
                                            .GroupBy(group => group.id)
                                            .Select(item => new MatchList
                                            {
                                                id = item.Key,
                                                pos = item.SelectMany(item => item.pos).ToList(),
                                                freq = item.Sum(item => item.freq),
                                                proxScore = CalcProximityScore(item.SelectMany(item => item.pos).ToList())
                                            })
                                            .ToList();
            return Rank(aggregate);
        }

        private static double CalcProximityScore(List<int> positions)
        {
            if (positions.Count <= 1)
            {
                return 0; //No proximity info here
            }

            positions.Sort();
            double score = 0;
            for (int i = 1; i < positions.Count; i++)
            {
                score += positions[i] - positions[i - 1];
            }
            double avgScore = score / (positions.Count - 1);

            //The smaller the average score, the higher the final proximity score
            return 1 / avgScore;
        }
    }

    public class DocMatch
    {
        [JsonPropertyName("word")]
        public string word { get; set; }

        [JsonPropertyName("matches")]
        public List<MatchList> matches { get; set; }
    }

    public class MatchList
    {
        [JsonPropertyName("docid")]
        public string id { get; set; }
        [JsonPropertyName("positions")]
        public List<int> pos { get; set; }
        [JsonPropertyName("freq")]
        public int freq { get; set; }

        public double proxScore;
    }
}