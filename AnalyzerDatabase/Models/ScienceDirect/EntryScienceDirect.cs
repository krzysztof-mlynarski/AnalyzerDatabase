using System.Collections.Generic;
using AnalyzerDatabase.Enums;
using AnalyzerDatabase.Interfaces;
using LiveCharts.Helpers;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.ScienceDirect
{
    public class EntryScienceDirect : ISearchResultsToDisplay
    {
        #region Variables
        [JsonProperty("dc:title")]
        public string Title { get; set; }

        [JsonProperty("prism:publicationName")]
        public string PublicationName { get; set; }

        [JsonProperty("prism:coverDisplayDate")]
        public string PublicationDate { get; set; }

        [JsonProperty("dc:creator")]
        public string Creator { get; set; }

        [JsonProperty("prism:volume")]
        public string Volume { get; set; }

        [JsonProperty("prism:issueIdentifier")]
        public string IssueIdentifier { get; set; }

        [JsonProperty("prism:doi")]
        public string Doi { get; set; }

        [JsonProperty("pii")]
        public string Pii { get; set; }

        [JsonProperty("eid")]
        public string Eid { get; set; }

        public string Arnumber { get; set; }

        [JsonProperty("prism:issn")]
        public string Issn { get; set; }

        [JsonProperty("dc:identifier")]
        public string Identifier { get; set; }

        [JsonProperty("openaccess")]
        public string OpenAccess { get; set; }

        [JsonProperty("prism:teaser")]
        public string Abstract { get; set; }

        public SourceDatabase Source { get; set; }
        public decimal PercentComplete { get; set; }
        public bool IsDuplicate { get; set; }
        public string Year { get; set; }

        //unused
        public string Isbn { get; set; }
        public string PageRange { get; set; }


        //JsonProperty
        [JsonProperty("@_fa")]
        public string Fa { get; set; }

        [JsonProperty("link")]
        public IList<LinkArticle> Link { get; set; }

        [JsonProperty("prism:url")]
        public string PrismUrl { get; set; }

        [JsonProperty("prism:coverDate")]
        public IList<PrismCoverDate> PrismCoverDate { get; set; }

        [JsonProperty("prism:startingPage")]
        public string PrismStartingPage { get; set; }

        [JsonProperty("prism:endingPage")]
        public string PrismEndingPage { get; set; }

        [JsonProperty("openaccessArticle")]
        public bool OpenaccessArticle { get; set; }

        [JsonProperty("openArchiveArticle")]
        public bool OpenArchiveArticle { get; set; }

        [JsonProperty("openaccessUserLicense")]
        public string OpenaccessUserLicense { get; set; }

        [JsonProperty("authors")]
        public Authors Authors { get; set; }
        #endregion

        #region Constructors
        public EntryScienceDirect(string fa, IList<LinkArticle> link, string dcIdentifier, string eid, string prismUrl, string dcTitle, string dcCreator, string prismPublicationName, string prismIssn, string prismVolume, string prismIssueIdentifier, IList<PrismCoverDate> prismCoverDate, string prismCoverDisplayDate, string prismStartingPage, string prismEndingPage, string prismDoi, string openaccess, bool openaccessArticle, bool openArchiveArticle, string openaccessUserLicense, string pii, Authors authors, string prismTeaser)
        {
            Fa = fa;
            Link = link;
            Identifier = dcIdentifier;
            Eid = eid;
            PrismUrl = prismUrl;
            Title = dcTitle;
            Creator = dcCreator;
            PublicationName = prismPublicationName;
            Issn = prismIssn;
            Volume = prismVolume;
            IssueIdentifier = prismIssueIdentifier;
            PrismCoverDate = prismCoverDate;
            PublicationDate = prismCoverDisplayDate;
            PrismStartingPage = prismStartingPage;
            PrismEndingPage = prismEndingPage;
            Doi = prismDoi;
            OpenAccess = openaccess;
            OpenaccessArticle = openaccessArticle;
            OpenArchiveArticle = openArchiveArticle;
            OpenaccessUserLicense = openaccessUserLicense;
            Pii = pii;
            Authors = authors;
            Abstract = prismTeaser;
            Source = SourceDatabase.ScienceDirect;
        }
        #endregion

        #region Public methods
        public List<string> GetCreator()
        {
            var list = new List<string>();
            Authors?.Author.ForEach(x => list.Add(x.GivenName + " " + x.Surname));

            return list;
        }
        #endregion
    }
}