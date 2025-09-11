using Newtonsoft.Json;
using System;

namespace POS_display.Models.Recipe.Classifiers
{
    public class CompensationTypeClassifier
    {
        [JsonProperty("KODAS")]
        public string Code { get; set; }

        [JsonProperty("PROCENTAS")]
        public int Percent { get; set; }

        [JsonProperty("RODOMA_REIKSME")]
        public string DisplayValue { get; set; }

        [JsonProperty("PRADZIA")]
        DateTime? StartDate { get; set; }

        [JsonProperty("PABAIGA")]
        DateTime? EndDate { get; set; }

        [JsonProperty("MODIF_DATA")]
        public DateTime? ModifyDate { get; set; }
    }
}
