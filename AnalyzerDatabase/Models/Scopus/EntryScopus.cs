using System.Collections.Generic;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Scopus
{
    public class EntryScopus
    {
        [JsonProperty("@_fa")]
        public string Fa { get; set; }

        [JsonProperty("link")]
        public IList<LinkArticle> Link { get; set; }

        [JsonProperty("prism:url")]
        public string PrismUrl { get; set; }

        [JsonProperty("dc:identifier")]
        public string DcIdentifier { get; set; }

        [JsonProperty("eid")]
        public string Eid { get; set; }

        [JsonProperty("dc:title")]
        public string DcTitle { get; set; }

        [JsonProperty("dc:creator")]
        public string DcCreator { get; set; }

        [JsonProperty("prism:publicationName")]
        public string PrismPublicationName { get; set; }

        [JsonProperty("prism:issn")]
        public string PrismIssn { get; set; }

        [JsonProperty("prism:eIssn")]
        public string PrismEIssn { get; set; }

        [JsonProperty("prism:volume")]
        public string PrismVolume { get; set; }

        [JsonProperty("prism:pageRange")]
        public string PrismPageRange { get; set; }

        [JsonProperty("prism:coverDate")]
        public string PrismCoverDate { get; set; }

        [JsonProperty("prism:coverDisplayDate")]
        public string PrismCoverDisplayDate { get; set; }

        [JsonProperty("prism:doi")]
        public string PrismDoi { get; set; }

        [JsonProperty("pii")]
        public string Pii { get; set; }

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

        [JsonProperty("prism:issueIdentifier")]
        public string PrismIssueIdentifier { get; set; }

        [JsonProperty("prism:isbn")]
        public string PrismIsbn { get; set; }

        [JsonProperty("article-number")]
        public string ArticleNumber { get; set; }

        public string Source { get; set; }

        public EntryScopus(string fa, IList<LinkArticle> link, string prismUrl, string dcIdentifier, string eid, string dcTitle, string dcCreator, string prismPublicationName, string prismIssn, string prismEIssn, string prismVolume, string prismPageRange, string prismCoverDate, string prismCoverDisplayDate, string prismDoi, string pii, string citedbyCount, IList<Affiliation> affiliation, string prismAggregationType, string subtype, string subtypeDescription, string sourceId, string prismIssueIdentifier, string prismIsbn, string articleNumber)
        {
            Fa = fa;
            Link = link;
            PrismUrl = prismUrl;
            DcIdentifier = dcIdentifier;
            Eid = eid;
            DcTitle = dcTitle;
            DcCreator = dcCreator;
            PrismPublicationName = prismPublicationName;
            PrismIssn = prismIssn;
            PrismEIssn = prismEIssn;
            PrismVolume = prismVolume;
            PrismPageRange = prismPageRange;
            PrismCoverDate = prismCoverDate;
            PrismCoverDisplayDate = prismCoverDisplayDate;
            PrismDoi = prismDoi;
            Pii = pii;
            CitedbyCount = citedbyCount;
            Affiliation = affiliation;
            PrismAggregationType = prismAggregationType;
            Subtype = subtype;
            SubtypeDescription = subtypeDescription;
            SourceId = sourceId;
            PrismIssueIdentifier = prismIssueIdentifier;
            PrismIsbn = prismIsbn;
            ArticleNumber = articleNumber;
            Source = "Scopus";
        }
    }
}