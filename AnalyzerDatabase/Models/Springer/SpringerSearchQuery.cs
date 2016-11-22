using System.Collections.Generic;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Springer
{
    public class SpringerSearchQuery
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("result")]
        public IList<Result> Result { get; set; }

        [JsonProperty("records")]
        public IList<Record> Records { get; set; }

        [JsonProperty("facets")]
        public IList<Facet> Facets { get; set; }

        public SpringerSearchQuery(string query, string apiKey, IList<Result> result, IList<Record> records, IList<Facet> facets)
        {
            Query = query;
            ApiKey = apiKey;
            Result = result;
            Records = records;
            Facets = facets;
        }
    }
}