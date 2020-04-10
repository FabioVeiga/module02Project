using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyReviewBook.Services
{

    internal class IndustryIdentifier
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }
    }

}
