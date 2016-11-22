using System;
using System.Collections.Generic;
using System.Windows;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.ScienceDirect
{
    public class ScienceDirectSearchQuery
    {
        [JsonProperty("search-results")]
        public SearchResults SearchResults { get; set; }

        public ScienceDirectSearchQuery(SearchResults searchResults)
        {
            SearchResults = searchResults;
        }
    }
}