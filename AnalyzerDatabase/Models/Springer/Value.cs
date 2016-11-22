using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Springer
{
    public class Value
    {
        [JsonProperty("value")]
        public string Values { get; set; }

        [JsonProperty("count")]
        public string Count { get; set; }

        public Value(string values, string count)
        {
            Values = values;
            Count = count;
        }
    }
}