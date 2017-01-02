using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Springer
{
    public class Value
    {
        #region Variables
        [JsonProperty("value")]
        public string Values { get; set; }

        [JsonProperty("count")]
        public string Count { get; set; }
        #endregion

        #region Constructors
        public Value(string values, string count)
        {
            Values = values;
            Count = count;
        }
        #endregion
    }
}