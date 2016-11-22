using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.ScienceDirect
{
    public class OpensearchQuery
    {
        [JsonProperty("@role")]
        public string Role { get; set; }

        [JsonProperty("@searchTerms")]
        public string SearchTerms { get; set; }

        [JsonProperty("@startPage")]
        public string StartPage { get; set; }

        public OpensearchQuery(string role, string searchTerms, string startPage)
        {
            Role = role;
            SearchTerms = searchTerms;
            StartPage = startPage;
        }
    }
}