using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Springer
{
    public class Result
    {
        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("start")]
        public string Start { get; set; }

        [JsonProperty("pageLength")]
        public string PageLength { get; set; }

        public Result(string total, string start, string pageLength)
        {
            Total = total;
            Start = start;
            PageLength = pageLength;
        }
    }
}