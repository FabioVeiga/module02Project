using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyReviewBook.Services
{

    internal class book
    {

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }

        [JsonProperty("items")]
        public IList<Item> Items { get; set; }
    }

}
