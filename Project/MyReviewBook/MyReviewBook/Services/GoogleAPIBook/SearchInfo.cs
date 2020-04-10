using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyReviewBook.Services
{

    internal class SearchInfo
    {

        [JsonProperty("textSnippet")]
        public string TextSnippet { get; set; }
    }

}
