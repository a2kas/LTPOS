using Newtonsoft.Json;

namespace POS_display.Models.Partner
{
    public class UploadSignatureResponse
    {
        [JsonProperty(PropertyName = "url")]
        public string PathToSignature { get; set; }
    }
}
