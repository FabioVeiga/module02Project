using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyReviewBook.Services
{

    internal class Pdf
    {

        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonProperty("acsTokenLink")]
        public string AcsTokenLink { get; set; }
    }

}
