using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace POS_display.Models.Recipe.Classifiers
{
    public class RouteClassifier
    {
        [JsonProperty("CODE")]
        public string Code { get; set; }

        [JsonProperty("VALIDFROM")]
        public DateTime? ValidFrom { get; set; }

        [JsonProperty("VALIDTO")]
        public DateTime? ValidTo { get; set; }

        [JsonProperty("PAVADINIMAI")]
        public List<RouteNameClassifier> Names { get; set; }

        [JsonProperty("MODIF_DATA")]
        public DateTime? ModifyDate { get; set; }
    }
}
