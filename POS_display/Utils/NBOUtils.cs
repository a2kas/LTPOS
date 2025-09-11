using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Utils
{
    public class NBOUtils
    {
        #region Members
        private readonly Uri baseAddress;
        private readonly Uri fullAddress;
        private Items.TamroGatewayAccessToken accessToken { get; set; }
        #endregion

        #region Construct
        public NBOUtils()
        {
            string version = Session.Develop ? Session.getParam("NBO_TEST", "VERSION") :
                                               Session.getParam("NBO", "VERSION");
            baseAddress = Session.Develop
                           ? new Uri(Session.getParam("NBO_TEST", "URL"))
                           : new Uri(Session.getParam("NBO", "URL"));
            fullAddress = new Uri(baseAddress.AbsoluteUri + $"api/{version}/");
        }
        #endregion

        #region Private methods
        private async Task GetToken()
        {
            if (accessToken?.created_at.AddSeconds(accessToken.expires_in) > DateTime.Now.AddSeconds(-10))
                return;
            var user = Session.Develop ? Session.getParam("NBO_TEST", "USER") : Session.getParam("NBO", "USER");
            var password = Session.Develop ? Session.getParam("NBO_TEST", "PASSWORD") : Session.getParam("NBO", "PASSWORD");

            var tokenContent = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", user),
                new KeyValuePair<string, string>("client_secret", password)
            };
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = baseAddress;
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                using (var response = await httpClient.PostAsync("api/auth/token", new FormUrlEncodedContent(tokenContent)))
                    accessToken = await response.Content.ReadAsAsync<Items.TamroGatewayAccessToken>();
            }
        }

        private async Task<T> Execute<T>(Func<HttpClient, Task<T>> func)
        {
            T result;
            await GetToken();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = fullAddress;
                httpClient.SetBearerToken(accessToken.access_token);
               
                result = await func(httpClient);
            }
            return result;
        }
        #endregion

        #region Public methods
        public async Task<T> PatchAsync<T>(string requestUri, object content)
        {
            return await Execute(async (httpClient) =>
            {
                using (var response = await httpClient.PatchAsync(requestUri, new StringContent(
                                JsonConvert.SerializeObject(content),
                                Encoding.UTF8,
                                "application/json")))
                    return await response.Content.ReadAsAsync<T>();
            });
        }

        public async Task<T> PutAsync<T>(string requestUri, object content)
        {
            return await Execute(async (httpClient) =>
            {
                using (var response = await httpClient.PutAsJsonAsync(requestUri, content))
                    return await response.Content.ReadAsAsync<T>();
            });
        }

        public async Task<T> GetAsync<T>(string url)
        {
            T result;
            return await Execute(async (httpClient) =>
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<T>(jsonString);
                    return result;

                }
            });
        }

        public async Task<T> PostAsync<T>(string requestUri, object content)
        {
            return await Execute(async (httpClient) =>
            {
                using (var response = await httpClient.PostAsJsonAsync(requestUri, content))
                    return await response.Content.ReadAsAsync<T>();
            });
        }
        #endregion
    }
}
