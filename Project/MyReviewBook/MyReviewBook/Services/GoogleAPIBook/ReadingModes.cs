using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyReviewBook.Services
{

    internal class ReadingModes
    {

        [JsonProperty("text")]
        public bool Text { get; set; }

        [JsonProperty("image")]
        public bool Image { get; set; }
    }

}
