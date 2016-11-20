using Newtonsoft.Json;

namespace AnalyzerDatabase.Models
{
    public class LinkPaging
    {
        [JsonProperty("@_fa")]
        public string Fa { get; set; }

        [JsonProperty("@href")]
        public string Href { get; set; }

        [JsonProperty("@ref")]
        public string Ref { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        public LinkPaging(string fa, string href, string @ref, string type)
        {
            Fa = fa;
            Href = href;
            Ref = @ref;
            Type = type;
        }
    }
}