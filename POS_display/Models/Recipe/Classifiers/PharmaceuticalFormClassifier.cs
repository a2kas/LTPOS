using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace POS_display.Models.Recipe.Classifiers
{
    public class PharmaceuticalFormClassifier
    {
        [JsonProperty("CODE")]
        public string Code { get; set; }

        [JsonProperty("VALIDFROM")]
        public DateTime? ValidFrom { get; set; }

        [JsonProperty("VALIDTO")]
        public DateTime? ValidTo { get; set; }

        [JsonProperty("PAVADINIMAI")]
        public List<PharmaceuticalFormNameClassifier> Names { get; set; }

        [JsonProperty("MODIF_DATA")]
        public DateTime? ModifyDate { get; set; }
    }
}
