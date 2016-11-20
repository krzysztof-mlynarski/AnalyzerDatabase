using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.ScienceDirect
{
    public class EntryScienceDirect
    {
        [JsonProperty("@_fa")]
        public string Fa { get; set; }

        [JsonProperty("link")]
        public IList<LinkArticle> Link { get; set; }

        [JsonProperty("dc:identifier")]
        public string DcIdentifier { get; set; }

        [JsonProperty("eid")]
        public string Eid { get; set; }

        [JsonProperty("prism:url")]
        public string PrismUrl { get; set; }

        [JsonProperty("dc:title")]
        public string DcTitle { get; set; }

        [JsonProperty("dc:creator")]
        public string DcCreator { get; set; }

        [JsonProperty("prism:publicationName")]
        public string PrismPublicationName { get; set; }

        [JsonProperty("prism:issn")]
        public string PrismIssn { get; set; }

        [JsonProperty("prism:volume")]
        public string PrismVolume { get; set; }

        [JsonProperty("prism:issueIdentifier")]
        public string PrismIssueIdentifier { get; set; }

        [JsonProperty("prism:coverDate")]
        public IList<PrismCoverDate> PrismCoverDate { get; set; }

        [JsonProperty("prism:coverDisplayDate")]
        public string PrismCoverDisplayDate { get; set; }

        [JsonProperty("prism:startingPage")]
        public string PrismStartingPage { get; set; }

        [JsonProperty("prism:endingPage")]
        public string PrismEndingPage { get; set; }

        [JsonProperty("prism:doi")]
        public string PrismDoi { get; set; }

        [JsonProperty("openaccess")]
        public string Openaccess { get; set; }

        [JsonProperty("openaccessArticle")]
        public bool OpenaccessArticle { get; set; }

        [JsonProperty("openArchiveArticle")]
        public bool OpenArchiveArticle { get; set; }

        [JsonProperty("openaccessUserLicense")]
        public string OpenaccessUserLicense { get; set; }

        [JsonProperty("pii")]
        public string Pii { get; set; }

        [JsonProperty("authors")]
        public Authors Authors { get; set; }

        [JsonProperty("prism:teaser")]
        public string PrismTeaser { get; set; }

        public string Source { get; set; }

        public EntryScienceDirect(string fa, IList<LinkArticle> link, string dcIdentifier, string eid, string prismUrl, string dcTitle, string dcCreator, string prismPublicationName, string prismIssn, string prismVolume, string prismIssueIdentifier, IList<PrismCoverDate> prismCoverDate, string prismCoverDisplayDate, string prismStartingPage, string prismEndingPage, string prismDoi, string openaccess, bool openaccessArticle, bool openArchiveArticle, string openaccessUserLicense, string pii, Authors authors, string prismTeaser)
        {
            Fa = fa;
            Link = link;
            DcIdentifier = dcIdentifier;
            Eid = eid;
            PrismUrl = prismUrl;
            DcTitle = dcTitle;
            DcCreator = dcCreator;
            PrismPublicationName = prismPublicationName;
            PrismIssn = prismIssn;
            PrismVolume = prismVolume;
            PrismIssueIdentifier = prismIssueIdentifier;
            PrismCoverDate = prismCoverDate;
            PrismCoverDisplayDate = prismCoverDisplayDate;
            PrismStartingPage = prismStartingPage;
            PrismEndingPage = prismEndingPage;
            PrismDoi = prismDoi;
            Openaccess = openaccess;
            OpenaccessArticle = openaccessArticle;
            OpenArchiveArticle = openArchiveArticle;
            OpenaccessUserLicense = openaccessUserLicense;
            Pii = pii;
            Authors = authors;
            PrismTeaser = prismTeaser;
            Source = "Science Direct";
        }
    }
}