using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace POS_display.Models.CRM
{
    public class GetCampaignItemsRequest
    {
        [JsonProperty(PropertyName = "company")]
        public string Company { get; set; }
        [JsonProperty(PropertyName = "dateFrom")]
        public DateTime DateFrom { get; set; }
        [JsonProperty(PropertyName = "dateTo")]
        public DateTime? DateTo { get; set; }
        [JsonProperty(PropertyName = "clientType")]
        public List<string> ClientType { get; set; }
        [JsonProperty(PropertyName = "systemId")]
        public string SystemId { get; set; }
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }
    }
}