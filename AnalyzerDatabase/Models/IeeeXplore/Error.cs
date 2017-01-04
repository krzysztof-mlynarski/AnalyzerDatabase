using System.Xml.Serialization;

namespace AnalyzerDatabase.Models.IeeeXplore
{
    [XmlRoot(ElementName = "Error")]
    public class Error
    {
        [XmlText]
        public string Text { get; set; }
    }
}