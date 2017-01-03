using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.ScienceDirect
{
    public class ScienceDirectSearchQuery
    {
        #region Variables
        [JsonProperty("search-results")]
        public SearchResults SearchResults { get; set; }
        #endregion

        #region Constructors
        public ScienceDirectSearchQuery(SearchResults searchResults)
        {
            SearchResults = searchResults;
        }
        #endregion
    }
}