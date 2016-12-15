using System.Xml.Serialization;
using AnalyzerDatabase.Enums;
using AnalyzerDatabase.Interfaces;

namespace AnalyzerDatabase.Models.IeeeXplore
{
    [XmlRoot(ElementName = "document")]
    public class Document : ISearchResultsToDisplay
    {
        [XmlElement(ElementName = "rank")]
        public string Rank { get; set; }

        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "authors")]
        public string Creator { get; set; }

        [XmlElement(ElementName = "affiliations")]
        public string Affiliations { get; set; }

        [XmlElement(ElementName = "controlledterms")]
        public Controlledterms Controlledterms { get; set; }

        [XmlElement(ElementName = "thesaurusterms")]
        public Thesaurusterms Thesaurusterms { get; set; }

        [XmlElement(ElementName = "pubtitle")]
        public string PublicationName { get; set; }

        [XmlElement(ElementName = "punumber")]
        public string Punumber { get; set; }

        [XmlElement(ElementName = "pubtype")]
        public string Pubtype { get; set; }

        [XmlElement(ElementName = "publisher")]
        public string Publisher { get; set; }

        [XmlElement(ElementName = "volume")]
        public string Volume { get; set; }

        public string IssueIdentifier { get; set; }

        [XmlElement(ElementName = "py")]
        public string PublicationDate { get; set; }

        [XmlElement(ElementName = "spage")]
        public string Spage { get; set; }

        [XmlElement(ElementName = "epage")]
        public string Epage { get; set; }

        public string PageRange { get; set; }

        [XmlElement(ElementName = "abstract")]
        public string Abstract { get; set; }

        public SourceDatabase Source { get; set; }
        public decimal PercentComplete { get; set; }
        public bool IsDuplicate { get; set; }
        public string Year { get; set; }

        [XmlElement(ElementName = "isbn")]
        public string Isbn { get; set; }

        public string Identifier { get; set; }
        public string OpenAccess { get; set; }

        [XmlElement(ElementName = "htmlFlag")]
        public string HtmlFlag { get; set; }

        [XmlElement(ElementName = "arnumber")]
        public string Arnumber { get; set; }

        [XmlElement(ElementName = "doi")]
        public string Doi { get; set; }

        public string Pii { get; set; }

        [XmlElement(ElementName = "publicationId")]
        public string PublicationId { get; set; }

        [XmlElement(ElementName = "partnum")]
        public string Partnum { get; set; }

        [XmlElement(ElementName = "mdurl")]
        public string Mdurl { get; set; }

        [XmlElement(ElementName = "pdf")]
        public string Pdf { get; set; }

        [XmlElement(ElementName = "issue")]
        public string Issue { get; set; }

        [XmlElement(ElementName = "issn")]
        public string Issn { get; set; }

    //    public Document(string rank, string title, string publicationDate, string creator, string affiliations, Controlledterms controlledterms, Thesaurusterms thesaurusterms, string publicationName, string punumber, string pubtype, string publisher, string volume, string issueIdentifier, string py, string spage, string epage, string pageRange, string @abstract, SourceDatabase source, decimal percentComplete, bool isDuplicate, string year, string isbn, string identifier, string openAccess, string htmlFlag, string arnumber, string doi, string pii, string publicationId, string partnum, string mdurl, string pdf, string issue, string issn)
    //    {
    //        Rank = rank;
    //        Title = title;
    //        PublicationDate = publicationDate;
    //        Creator = creator;
    //        Affiliations = affiliations;
    //        Controlledterms = controlledterms;
    //        Thesaurusterms = thesaurusterms;
    //        PublicationName = publicationName;
    //        Punumber = punumber;
    //        Pubtype = pubtype;
    //        Publisher = publisher;
    //        Volume = volume;
    //        IssueIdentifier = issueIdentifier;
    //        Py = py;
    //        Spage = spage;
    //        Epage = epage;
    //        PageRange = pageRange;
    //        Abstract = @abstract;
    //        Source = SourceDatabase.IeeeXplore;
    //        PercentComplete = percentComplete;
    //        IsDuplicate = isDuplicate;
    //        Year = year;
    //        Isbn = isbn;
    //        Identifier = identifier;
    //        OpenAccess = openAccess;
    //        HtmlFlag = htmlFlag;
    //        Arnumber = arnumber;
    //        Doi = doi;
    //        Pii = pii;
    //        PublicationId = publicationId;
    //        Partnum = partnum;
    //        Mdurl = mdurl;
    //        Pdf = pdf;
    //        Issue = issue;
    //        Issn = issn;
    //    }
    }
}