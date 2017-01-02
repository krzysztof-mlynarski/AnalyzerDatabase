using System;
using System.Xml.Serialization;

namespace AnalyzerDatabase.Models
{
    [Serializable]
    public class AnalyzerDatabaseSettings
    {
        #region Variables
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