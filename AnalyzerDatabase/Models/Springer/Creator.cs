using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Springer
{
    public class Creator
    {
        [JsonProperty("creator")]
        public string Creators { get; set; }

        public Creator(string creator)
        {
            Creators = creator;
        }
    }
}