using System.Xml.Serialization;

namespace AnalyzerDatabase.Models
{
    public class AnalyzerDatabaseStatistics
    {
        [XmlElement("ScienceDirectCount")]
        public int ScienceDirectCount { get; set; }

        [XmlElement("ScopusCount")]
        public int ScopusCount { get; set; }

        [XmlElement("SpringerCount")]
        public int SpringerCount { get; set; }

        [XmlElement("WebOfScienceCount")]
        public int WebOfScienceCount { get; set; }

        [XmlElement("IeeeXploreCount")]
        public int IeeeXploreCount { get; set; }

        [XmlElement("WileyOnlineLibraryCount")]
        public int WileyOnlineLibraryCount { get; set; }

        [XmlElement("DuplicateCount")]
        public int DuplicateCount { get; set; }

        [XmlElement("PublicationsDownloadCount")]
        public int PublicationsDownloadCount { get; set; }

        [XmlElement("SumCount")]
        public int SumCount { get; set; }
    }
}