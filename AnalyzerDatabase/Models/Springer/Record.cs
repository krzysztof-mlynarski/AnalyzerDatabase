using System.Collections.Generic;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Models.Springer
{
    public class Record
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("url")]
        public IList<Url> Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("creators")]
        public IList<Creator> Creators { get; set; }

        [JsonProperty("publicationName")]
        public string PublicationName { get; set; }

        [JsonProperty("openaccess")]
        public string Openaccess { get; set; }

        [JsonProperty("doi")]
        public string Doi { get; set; }

        [JsonProperty("printIsbn")]
        public string PrintIsbn { get; set; }

        [JsonProperty("electronicIsbn")]
        public string ElectronicIsbn { get; set; }

        [JsonProperty("isbn")]
        public string Isbn { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("publicationDate")]
        public string PublicationDate { get; set; }

        [JsonProperty("volume")]
        public string Volume { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("startingPage")]
        public string StartingPage { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("abstract")]
        public string Abstract { get; set; }

        public string Source { get; set; }

        public Record(string identifier, IList<Url> url, string title, IList<Creator> creators, string publicationName, string openaccess, string doi, string printIsbn, string electronicIsbn, string isbn, string publisher, string publicationDate, string volume, string number, string startingPage, string copyright, string genre, string @abstract)
        {
            Identifier = identifier;
            Url = url;
            Title = title;
            Creators = creators;
            PublicationName = publicationName;
            Openaccess = openaccess;
            Doi = doi;
            PrintIsbn = printIsbn;
            ElectronicIsbn = electronicIsbn;
            Isbn = isbn;
            Publisher = publisher;
            PublicationDate = publicationDate;
            Volume = volume;
            Number = number;
            StartingPage = startingPage;
            Copyright = copyright;
            Genre = genre;
            Abstract = @abstract;
            Source = "Springer";
        }
    }
}