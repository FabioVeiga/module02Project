using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyReviewBook.Services
{

    internal class PanelizationSummary
    {

        [JsonProperty("containsEpubBubbles")]
        public bool ContainsEpubBubbles { get; set; }

        [JsonProperty("containsImageBubbles")]
        public bool ContainsImageBubbles { get; set; }
    }

}
