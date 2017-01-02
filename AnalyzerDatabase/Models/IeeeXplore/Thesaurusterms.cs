using System.Collections.Generic;
using System.Xml.Serialization;

namespace AnalyzerDatabase.Models.IeeeXplore
{
    [XmlRoot(ElementName = "thesaurusterms")]
    public class Thesaurusterms
    {
        #region Variables
        [XmlElement(ElementName = "term")]
        public List<string> Term { get; set; }
        #endregion
    }
}