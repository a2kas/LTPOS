using Newtonsoft.Json;
using System;

namespace POS_display.Models.General
{
    public class MarkCommandRequest
    {
        [JsonProperty(PropertyName = "clientId")]
        public decimal ClientId { get; set; }
        [JsonProperty(PropertyName = "command")]
        public string CommandPrefix { get; set; }
        [JsonProperty(PropertyName = "from")]
        public DateTime FromDate { get; set; }
    }
}
