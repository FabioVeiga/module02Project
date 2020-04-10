using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyReviewBook.Services
{

    internal class VolumeInfo
    {

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("authors")]
        public IList<string> Authors { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("industryIdentifiers")]
        public IList<IndustryIdentifier> IndustryIdentifiers { get; set; }

        [JsonProperty("readingModes")]
        public ReadingModes ReadingModes { get; set; }

        [JsonProperty("pageCount")]
        public int PageCount { get; set; }

        [JsonProperty("printType")]
        public string PrintType { get; set; }

        [JsonProperty("categories")]
        public IList<string> Categories { get; set; }

        [JsonProperty("averageRating")]
        public double AverageRating { get; set; }

        [JsonProperty("ratingsCount")]
        public int RatingsCount { get; set; }

        [JsonProperty("maturityRating")]
        public string MaturityRating { get; set; }

        [JsonProperty("allowAnonLogging")]
        public bool AllowAnonLogging { get; set; }

        [JsonProperty("contentVersion")]
        public string ContentVersion { get; set; }

        [JsonProperty("panelizationSummary")]
        public PanelizationSummary PanelizationSummary { get; set; }

        [JsonProperty("imageLinks")]
        public ImageLinks ImageLinks { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("previewLink")]
        public string PreviewLink { get; set; }

        [JsonProperty("infoLink")]
        public string InfoLink { get; set; }

        [JsonProperty("canonicalVolumeLink")]
        public string CanonicalVolumeLink { get; set; }
    }

}
