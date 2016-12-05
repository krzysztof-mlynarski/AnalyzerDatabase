using AnalyzerDatabase.Interfaces;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Springer
{
    public class Result : ITotalResultsToDisplay
    {
        [JsonProperty("total")]
        public string OpensearchTotalResults { get; set; }

        [JsonProperty("start")]
        public string Start { get; set; }

        [JsonProperty("pageLength")]
        public string PageLength { get; set; }

        public Result(string total, string start, string pageLength)
        {
            OpensearchTotalResults = total;
            Start = start;
            PageLength = pageLength;
        }
    }
}