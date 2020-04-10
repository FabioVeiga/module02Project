﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyReviewBook.Services
{

    internal class RetailPrice2
    {

        [JsonProperty("amountInMicros")]
        public double AmountInMicros { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }

}
