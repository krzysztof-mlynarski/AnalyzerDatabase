using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.ScienceDirect
{
    public class Author
    {
        #region Variables
        [JsonProperty("@_fa")]
        public string Fa { get; set; }

        [JsonProperty("@given-name")]
        public string GivenName { get; set; }

        [JsonProperty("@surname")]
        public string Surname { get; set; }
        #endregion

        #region Constructors
        public Author(string fa, string givenName, string surname)
        {
            Fa = fa;
            GivenName = givenName;
            Surname = surname;
        }
        #endregion
    }
}