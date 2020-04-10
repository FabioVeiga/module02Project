
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyReviewBook.Services
{

    internal class ImageLinks
    {

        [JsonProperty("smallThumbnail")]
        public string SmallThumbnail { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
    }

}
