using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyReviewBook.Services
{

    internal class Offer
    {

        [JsonProperty("finskyOfferType")]
        public int FinskyOfferType { get; set; }

        [JsonProperty("listPrice")]
        public ListPrice2 ListPrice { get; set; }

        [JsonProperty("retailPrice")]
        public RetailPrice2 RetailPrice { get; set; }

        [JsonProperty("giftable")]
        public bool Giftable { get; set; }
    }

}
