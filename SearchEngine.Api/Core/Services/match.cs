using SearchEngine.Models;

namespace SearchEngine.Api.Core.Services
{
    class DocResult
    {
        private readonly DocumentService _documentService;
        public DocResult(DocumentService documentService) {
      _documentService = documentService;
    }

        /// <summary>
        /// Ranks the list of matches and returns a List of Match objects sorted in Descending Order (Largest comes first)
        /// </summary>
        /// <param name="matches">Matches to be ranked</param>
        /// <returns>Ranked result as a list of Match objects</returns>

        public List<MatchResult> Rank(List<Match> matches)
        {
            var rankedMatches = matches
                .Select(match => new
                {
                    Match = match,
                    Score = (match.Positions.Count * 0.7) + (CalcProximityScore(match.Positions) * 0.3),
                    Rank = ((match.Positions.Count * 0.7) + (CalcProximityScore(match.Positions) * 0.3)) * 10,
                })
                .OrderByDescending(x => x.Score)
                .Select(x => new MatchResult {
                  Rank = x.Rank,
                  DocId = x.Match.DocId,
                  Link = _documentService.FindDocumentLink(x.Match.DocId)
                })
                .ToList();

            return rankedMatches;
        }

        /// <summary>
        /// Parses all words and matches into a single easy to rank form
        /// </summary>
        /// <param name="res">results to be parsed</param>
        /// <returns>Final ranked aggregate</returns>
        public List<MatchResult> RankAll(List<WordIndexer> res)
        {
            List<Match> aggregate = res.SelectMany(items => items.Matches)
                                        .GroupBy(group => group.DocId)
                                        .Select(item => new Match
                                        {
                                            DocId = item.Key,
                                            Positions = item.SelectMany(subItem => subItem.Positions).ToList()
                                        })
                                        .ToList();
            return Rank(aggregate);
        }

        /// <summary>
        /// Gets the proximity score of words in a Doc
        /// </summary>
        /// <param name="positions">List of positions in Doc</param>
        /// <returns>Proximity score as a double</returns>
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
            return avgScore > 0 ? 1 / avgScore : double.MaxValue; // Avoid division by zero
        }
    }
}
