using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Springer
{
    public class Creator
    {
        #region Variables
        [JsonProperty("creator")]
        public string Creators { get; set; }
        #endregion

        #region Constructors    
        public Creator (string creator)
        {
            Creators = creator;
        }
        #endregion
    }
}