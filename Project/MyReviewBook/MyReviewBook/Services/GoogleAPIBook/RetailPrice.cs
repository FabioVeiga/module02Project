using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyReviewBook.Services
{

    internal class RetailPrice
    {

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }

}
