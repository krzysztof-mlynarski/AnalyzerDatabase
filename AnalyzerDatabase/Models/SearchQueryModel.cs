using Newtonsoft.Json;

namespace AnalyzerDatabase.Models
{
    public class SearchQueryModel
    {
        [JsonProperty(PropertyName = "totalResult")]
        public string TotalResult { get; set; }

        [JsonProperty(PropertyName = "identifier")]
        public string Identifier { get; set; } //numer DOI

        [JsonProperty(PropertyName = "url")] 
        public string Url { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "creator")]
        public string Creator { get; set; }

        [JsonProperty(PropertyName = "issn")]
        public string Issn { get; set; }  //issn i isbn

        [JsonProperty(PropertyName = "pii")] 
        public string Pii { get; set; }

        public SearchQueryModel(string totalResult, string identifier, string url, string title, string creator, string issn, string pii)
        {
            this.TotalResult = totalResult;
            this.Identifier = identifier;
            this.Url = url;
            this.Title = title;
            this.Creator = creator;
            this.Issn = issn;
            this.Pii = pii;
        }
    }
}