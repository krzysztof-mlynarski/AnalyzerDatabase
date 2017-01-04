using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.ScienceDirect
{
    public class PrismCoverDate
    {
        #region Variables
        [JsonProperty("@_fa")]
        public string Fa { get; set; }

        [JsonProperty("$")]
        public string Date { get; set; }
        #endregion

        #region Constructors
        public PrismCoverDate(string fa, string date)
        {
            Fa = fa;
            Date = date;
        }
        #endregion
    }
}