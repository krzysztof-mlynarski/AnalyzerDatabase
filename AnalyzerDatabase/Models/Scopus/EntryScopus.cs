using System.Collections.Generic;
using AnalyzerDatabase.Enums;
using AnalyzerDatabase.Interfaces;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Scopus
{
    public class EntryScopus : ISearchResultsToDisplay
    {
        [JsonProperty("dc:title")]
        public string Title { get; set; }

        [JsonProperty("prism:publicationName")]
        public string PublicationName { get; set; }

        [JsonProperty("prism:coverDisplayDate")]
        public string PublicationDate { get; set; }

        [JsonProperty("dc:creator")]
        public string Creator { get; set; }

        //public IList<Creator> Creators { get; set; }

        [JsonProperty("prism:volume")]
        public string Volume { get; set; }

        [JsonProperty("prism:issueIdentifier")]
        public string IssueIdentifier { get; set; }

        [JsonProperty("prism:doi")]
        public string Doi { get; set; }

        [JsonProperty("pii")]
        public string Pii { get; set; }

        [JsonProperty("prism:issn")]
        public string Issn { get; set; }

        [JsonProperty("dc:identifier")]
        public string Identifier { get; set; }

        [JsonProperty("prism:pageRange")]
        public string PageRange { get; set; }

        public SourceDatabase Source { get; set; }
        public decimal PercentComplete { get; set; }
        public bool IsDuplicate { get; set; }

        //not implemented
        public string OpenAccess { get; set; }

        public string Abstract { get; set; }



        //JsonProperty
        [JsonProperty("@_fa")]
        public string Fa { get; set; }

        [JsonProperty("link")]
        public IList<LinkArticle> Link { get; set; }

        [JsonProperty("prism:url")]
        public string PrismUrl { get; set; }

        [JsonProperty("eid")]
        public string Eid { get; set; }

        [JsonProperty("prism:eIssn")]
        public string PrismEIssn { get; set; }

        [JsonProperty("prism:coverDate")]
        public string PrismCoverDate { get; set; }

        [JsonProperty("citedby-count")]
        public string CitedbyCount { get; set; }

        [JsonProperty("affiliation")]
        public IList<Affiliation> Affiliation { get; set; }

        [JsonProperty("prism:aggregationType")]
        public string PrismAggregationType { get; set; }

        [JsonProperty("subtype")]
        public string Subtype { get; set; }

        [JsonProperty("subtypeDescription")]
        public string SubtypeDescription { get; set; }

        [JsonProperty("source-id")]
        public string SourceId { get; set; }

        [JsonProperty("prism:isbn")]
        public string Isbn { get; set; }

        [JsonProperty("article-number")]
        public string ArticleNumber { get; set; }

        public EntryScopus(string fa, IList<LinkArticle> link, string prismUrl, string dcIdentifier, string eid, string dcTitle, string dcCreator, string prismPublicationName, string prismIssn, string prismEIssn, string prismVolume, string prismPageRange, string prismCoverDate, string prismCoverDisplayDate, string prismDoi, string pii, string citedbyCount, IList<Affiliation> affiliation, string prismAggregationType, string subtype, string subtypeDescription, string sourceId, string prismIssueIdentifier, string prismIsbn, string articleNumber)
        {
            Fa = fa;
            Link = link;
            PrismUrl = prismUrl;
            Identifier = dcIdentifier;
            Eid = eid;
            Title = dcTitle;
            Creator = dcCreator;
            PublicationName = prismPublicationName;
            Issn = prismIssn;
            PrismEIssn = prismEIssn;
            Volume = prismVolume;
            PageRange = prismPageRange;
            PrismCoverDate = prismCoverDate;
            PublicationDate = prismCoverDisplayDate;
            Doi = prismDoi;
            Pii = pii;
            CitedbyCount = citedbyCount;
            Affiliation = affiliation;
            PrismAggregationType = prismAggregationType;
            Subtype = subtype;
            SubtypeDescription = subtypeDescription;
            SourceId = sourceId;
            IssueIdentifier = prismIssueIdentifier;
            Isbn = prismIsbn;
            ArticleNumber = articleNumber;
            Source = SourceDatabase.Scopus;
        }
    }
}