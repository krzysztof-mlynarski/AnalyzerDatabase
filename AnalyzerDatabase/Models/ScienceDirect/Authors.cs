using System.Collections.Generic;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.ScienceDirect
{
    public class Authors
    {
        [JsonProperty("author")]
        public IList<Author> Author { get; set; }

        public Authors(IList<Author> author)
        {
            Author = author;
        }
    }
}