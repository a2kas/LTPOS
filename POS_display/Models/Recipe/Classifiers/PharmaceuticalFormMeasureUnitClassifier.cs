using Newtonsoft.Json;
using System;

namespace POS_display.Models.Recipe.Classifiers
{
    public class PharmaceuticalFormMeasureUnitClassifier
    {
        [JsonProperty("VALID_FROM")]
        public DateTime? ValidFrom { get; set; }

        [JsonProperty("SYS_MODIFY_TIME")]
        public DateTime? SystemModifyTime { get; set; }

        [JsonProperty("CLASS_CODE")]
        public string ClassCode { get; set; }

        [JsonProperty("DISPLAY_CODE")]
        public string DisplayCode { get; set; }

        [JsonProperty("DISPLAY_NAME")]
        public string DisplayName { get; set; }

        [JsonProperty("TYPE_CODE")]
        public string TypeCode { get; set; }

        [JsonProperty("VALID_TO")]
        public DateTime? ValidTo { get; set; } 
    }
}
