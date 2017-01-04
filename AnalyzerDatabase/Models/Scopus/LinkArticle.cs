using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Scopus
{
    public class LinkArticle
    {
        #region Variables
        [JsonProperty("@_fa")]
        public string Fa { get; set; }

        [JsonProperty("@ref")]
        public string Ref { get; set; }

        [JsonProperty("@href")]
        public string Href { get; set; }
        #endregion

        #region Constructors
        public LinkArticle(string fa, string @ref, string href)
        {
            Fa = fa;
            Ref = @ref;
            Href = href;
        }
        #endregion
    }
}