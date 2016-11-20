using Newtonsoft.Json;

namespace AnalyzerDatabase.Models
{
    public class PrismCoverDate
    {
        [JsonProperty("@_fa")]
        public string Fa { get; set; }

        [JsonProperty("$")]
        public string Date { get; set; }

        public PrismCoverDate(string fa, string date)
        {
            Fa = fa;
            Date = date;
        }
    }
}