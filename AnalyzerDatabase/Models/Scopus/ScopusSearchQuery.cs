using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Scopus
{
    public class ScopusSearchQuery
    {
        #region Variables
        [JsonProperty("search-results")]
        public SearchResults SearchResults { get; set; }
        #endregion

        #region Constructors
        public ScopusSearchQuery(SearchResults searchResults)
        {
            SearchResults = searchResults;
        }
        #endregion
    }
}