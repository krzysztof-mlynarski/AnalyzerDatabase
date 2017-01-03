using System.Collections.Generic;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.ScienceDirect
{
    public class Authors
    {
        #region Variables
        [JsonProperty("author")]
        public IList<Author> Author { get; set; }
        #endregion

        #region Constructors
        public Authors(IList<Author> author)
        {
            Author = author;
        }
        #endregion
    }
}