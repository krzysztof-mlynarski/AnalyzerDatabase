using System.Collections.Generic;
using AnalyzerDatabase.Interfaces;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.ScienceDirect
{
    public class SearchResults : ITotalResultsToDisplay
    {
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
        public IList<EntryScienceDirect> Entry { get; set; }

        public SearchResults(string opensearchTotalResults, string opensearchStartIndex, string opensearchItemsPerPage, OpensearchQuery opensearchQuery, IList<LinkPaging> link, IList<EntryScienceDirect> entry)
        {
            OpensearchTotalResults = opensearchTotalResults;
            Start = opensearchStartIndex;
            OpensearchItemsPerPage = opensearchItemsPerPage;
            OpensearchQuery = opensearchQuery;
            Link = link;
            Entry = entry;
        }
    }
}