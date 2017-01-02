using System.Collections.Generic;
using AnalyzerDatabase.Interfaces;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Scopus
{
    public class SearchResults : ITotalResultsToDisplay
    {
        #region Variables
        [JsonProperty("opensearch:totalResults")]
        public string OpensearchTotalResults { get; set; }

        [JsonProperty("opensearch:startIndex")]
        public string Start { get; set; }

        [JsonProperty("opensearch:itemsPerPage")]
        public string OpensearchItemsPerPage { get; set; }

        [JsonProperty("opensearch:Query")]
        public OpensearchQuery OpensearchQuery { get; set; }

        [JsonProperty("link")]
        public IList<LinkPaging> Link { get; set; }

        [JsonProperty("entry")]
        public IList<EntryScopus> Entry { get; set; }
        #endregion

        #region Constructors
        public SearchResults(string opensearchTotalResults, string opensearchStartIndex, string opensearchItemsPerPage, OpensearchQuery opensearchQuery, IList<LinkPaging> link, IList<EntryScopus> entry)
        {
            OpensearchTotalResults = opensearchTotalResults;
            Start = opensearchStartIndex;
            OpensearchItemsPerPage = opensearchItemsPerPage;
            OpensearchQuery = opensearchQuery;
            Link = link;
            Entry = entry;
        }
        #endregion
    }
}