using Newtonsoft.Json;

namespace AnalyzerDatabase.Models
{
    public class LinkArticle
    {
        [JsonProperty("@_fa")]
        public string Fa { get; set; }

        [JsonProperty("@href")]
        public string Href { get; set; }

        [JsonProperty("@ref")]
        public string Ref { get; set; }

        public LinkArticle(string fa, string href, string @ref)
        {
            Fa = fa;
            Href = href;
            Ref = @ref;
        }
    }
}