using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.ScienceDirect
{
    public class LinkPaging
    {
        #region Variables
        [JsonProperty("@_fa")]
        public string Fa { get; set; }

        [JsonProperty("@href")]
        public string Href { get; set; }

        [JsonProperty("@ref")]
        public string Ref { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }
        #endregion

        #region Constructors
        public LinkPaging(string fa, string href, string @ref, string type)
        {
            Fa = fa;
            Href = href;
            Ref = @ref;
            Type = type;
        }
        #endregion
    }
}