using System.Collections.Generic;
using AnalyzerDatabase.Enums;
using AnalyzerDatabase.Interfaces;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Springer
{
    public class Record : IScienceDirectAndScopus
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("publicationName")]
        public string PublicationName { get; set; }

        [JsonProperty("publicationDate")]
        public string PublicationDate { get; set; }

        [JsonProperty("publisher")]
        public string Creator { get; set; }

        [JsonProperty("creators")]
        public IList<Creator> Creators { get; set; }

        [JsonProperty("volume")]
        public string Volume { get; set; }

        [JsonProperty("number")]
        public string IssueIdentifier { get; set; }

        [JsonProperty("doi")]
        public string Doi { get; set; }

        [JsonProperty("isbn")]
        public string Isbn { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("openaccess")]
        public string OpenAccess { get; set; }

        [JsonProperty("startingPage")]
        public string PageRange { get; set; }

        [JsonProperty("abstract")]
        public string Abstract { get; set; }

        public SourceDatabase Source { get; set; }

        //unused
        public string Pii { get; set; }
        public string Issn { get; set; }


        //JsonProperty
        [JsonProperty("url")]
        public IList<Url> Url { get; set; }

        [JsonProperty("printIsbn")]
        public string PrintIsbn { get; set; }

        [JsonProperty("electronicIsbn")]
        public string ElectronicIsbn { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        public Record(string identifier, IList<Url> url, string title, IList<Creator> creators, string publicationName, string openaccess, string doi, string printIsbn, string electronicIsbn, string isbn, string publisher, string publicationDate, string volume, string number, string startingPage, string copyright, string genre, string @abstract)
        {
            Identifier = identifier;
            Url = url;
            Title = title;
            Creators = creators;
            PublicationName = publicationName;
            OpenAccess = openaccess;
            Doi = doi;
            PrintIsbn = printIsbn;
            ElectronicIsbn = electronicIsbn;
            Isbn = isbn;
            Creator = publisher;
            PublicationDate = publicationDate;
            Volume = volume;
            IssueIdentifier = number;
            PageRange = startingPage;
            Copyright = copyright;
            Genre = genre;
            Abstract = @abstract;
            Source = SourceDatabase.Springer;
        }
    }
}