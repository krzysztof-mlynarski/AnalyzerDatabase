using System.Collections.Generic;
using System.Xml.Serialization;

namespace AnalyzerDatabase.Models.IeeeXplore
{
    [XmlRoot(ElementName = "controlledterms")]
    public class Controlledterms
    {
        [XmlElement(ElementName = "term")]
        public List<string> Term { get; set; }
    }
}