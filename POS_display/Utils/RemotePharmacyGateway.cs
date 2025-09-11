using Newtonsoft.Json;
using POS_display.Utils.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Utils
{
    public class RemotePharmacyGateway
    {
        private readonly Uri _baseAddress;
        private static HttpClient _httpClient = new HttpClient();

        public RemotePharmacyGateway()
        {
            _baseAddress = Session.Develop
                ? new Uri(Session.getParam("REMOTEPHARMACY_TEST", "URL"))
                : new Uri(Session.getParam("REMOTEPHARMACY", "URL"));
        }

        public async Task<T> PostCallApi<T>(string content)
        {
            try
            {
                using (var data = new StringContent(content, Encoding.Default, "application/json"))
                {
                    data.Headers.ContentType.CharSet = string.Empty;
                    var response = await _httpClient.PostAsync(_baseAddress, data);
                    if (response != null)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return JsonConvert.DeserializeObject<T>(jsonString);
                    }
                }
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return default;
            }
            return default;
        }
    }
}
