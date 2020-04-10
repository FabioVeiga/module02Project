using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyReviewBook.Services
{

    internal class SaleInfo
    {

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("saleability")]
        public string Saleability { get; set; }

        [JsonProperty("isEbook")]
        public bool IsEbook { get; set; }

        [JsonProperty("listPrice")]
        public ListPrice ListPrice { get; set; }

        [JsonProperty("retailPrice")]
        public RetailPrice RetailPrice { get; set; }

        [JsonProperty("buyLink")]
        public string BuyLink { get; set; }

        [JsonProperty("offers")]
        public IList<Offer> Offers { get; set; }
    }

}
