using Newtonsoft.Json;

namespace POS_display.Models.Partner
{
    public class SetAgreementRequest
    {
        [JsonProperty(PropertyName = "partnerId")]
        public decimal PartnerId { get; set; }
        [JsonProperty(PropertyName = "signaturePath")]
        public string PathToSignature { get; set; }
    }
}
