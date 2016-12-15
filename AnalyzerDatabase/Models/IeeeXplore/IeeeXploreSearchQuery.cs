using System.Collections.Generic;
using System.Xml.Serialization;
using AnalyzerDatabase.Interfaces;

namespace AnalyzerDatabase.Models.IeeeXplore
{
    [XmlRoot(ElementName = "root")]
    public class IeeeXploreSearchQuery : ITotalResultsToDisplay
    {
        [XmlElement(ElementName = "totalfound")]
        public string OpensearchTotalResults { get; set; }

        [XmlElement(ElementName = "totalsearched")]
        public string Totalsearched { get; set; }

        [XmlElement(ElementName = "document")]
        public List<Document> Document { get; set; }
    }
}