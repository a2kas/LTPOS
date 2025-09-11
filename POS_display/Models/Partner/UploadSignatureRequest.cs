using Newtonsoft.Json;

namespace POS_display.Models.Partner
{
    public class UploadSignatureRequest
    {
        [JsonProperty(PropertyName = "partnerId")]
        public decimal PartnerId { get; set; }
        [JsonProperty(PropertyName = "signature")]
        public string Base64Signature { get; set; }
    }
}
