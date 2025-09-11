using Newtonsoft.Json;
using System;

namespace POS_display.Models.Recipe.Classifiers
{
    public class TLK10AMClassifier
    {
        [JsonProperty("ID")]
        public long Id { get; set; }

        [JsonProperty("IS_PUBLISHED")]
        public bool IsPublished { get; set; }

        [JsonProperty("IS_DELETED")]
        public bool IsDeleted { get; set; }

        [JsonProperty("VALID_FROM")]
        public DateTime? ValidFrom { get; set; }

        [JsonProperty("VALID_UNTIL")]
        public DateTime? ValidUntil { get; set; }

        [JsonProperty("CREATED_ON")]
        public DateTime? CreatedOn { get; set; }

        [JsonProperty("MODIFIED_ON")]
        public DateTime? ModifiedOn { get; set; }

        [JsonProperty("SECTION_ID")]
        public long? SectionId { get; set; }

        [JsonProperty("PARENT_ID")]
        public long? ParentId { get; set; }

        [JsonProperty("CODE_LEVEL")]
        public int CodeLevel { get; set; }

        [JsonProperty("CIRCLED_TIMES_SYMBOL")]
        public bool CircledTimesSymbol{ get; set; }

        [JsonProperty("DAGGER_SYMBOL")]
        public bool DaggerSymbol { get; set; }

        [JsonProperty("ASTERISK_SYMBOL")]
        public bool AsteriskSymbol { get; set; }

        [JsonProperty("TITLE")]
        public string Title { get; set; }

        [JsonProperty("CODE")]
        public string Code { get; set; }
    }
}
