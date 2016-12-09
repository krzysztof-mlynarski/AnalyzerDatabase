using System;
using System.Xml.Serialization;

namespace AnalyzerDatabase.Models
{
    [Serializable]
    public class AnalyzerDatabaseSettings
    {
        #region Variables
        [XmlElement("CurrentLanguage")]
        public string CurrentLanguage { get; set; }

        [XmlElement("CurrentStyle")]
        public string CurrentStyle { get; set; }

        [XmlElement("ScienceDirectAndScopusApiKey")]
        public string ScienceDirectAndScopusApiKey { get; set; }

        [XmlElement("SpringerApiKey")]
        public string SpringerApiKey { get; set; }

        [XmlElement("StartOnLogin")]
        public bool StartOnLogin { get; set; }

        [XmlElement("SavingPublicationPath")]
        public string SavingPublicationPath { get; set; }
        #endregion
    }
}