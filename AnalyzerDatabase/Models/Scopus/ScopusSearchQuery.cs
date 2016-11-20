using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Scopus
{
    public class ScopusSearchQuery
    {
        [JsonProperty("search-results")]
        public SearchResults SearchResults { get; set; }

        public ScopusSearchQuery(SearchResults searchResults)
        {
            SearchResults = searchResults;
        }
    }
}