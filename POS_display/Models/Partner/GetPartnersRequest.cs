using Newtonsoft.Json;

namespace POS_display.Models.Partner
{
    public class GetPartnersRequest
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "ECode")]
        public string ECode { get; set; }
        [JsonProperty(PropertyName = "TCode")]
        public string TCode { get; set; }
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "Phone")]
        public string Phone { get; set; }
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }
    }
}
