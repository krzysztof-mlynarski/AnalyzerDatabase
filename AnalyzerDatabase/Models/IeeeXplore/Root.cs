using System.Collections.Generic;
using System.Xml.Serialization;

namespace AnalyzerDatabase.Models.IeeeXplore
{
    [XmlRoot(ElementName = "root")]
    public class Root
    {
        [XmlElement(ElementName = "totalfound")]
        public string Totalfound { get; set; }

        [XmlElement(ElementName = "totalsearched")]
        public string Totalsearched { get; set; }

        [XmlElement(ElementName = "document")]
        public List<Document> Document { get; set; }
    }
}