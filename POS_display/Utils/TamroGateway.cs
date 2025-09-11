using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace POS_display.Utils
{
    public class TamroGateway
    {
        private readonly Uri baseAddress;
        private Items.TamroGatewayAccessToken accessToken { get; set; }

        public TamroGateway()
        {
            if (Session.Develop == true)
                baseAddress = new Uri(Session.getParam("TAMRO_TEST", "URL"));
            else
                baseAddress = new Uri(Session.getParam("TAMRO", "URL"));
        }

        public async Task GetToken()
        {
            if (accessToken?.created_at.AddSeconds(accessToken.expires_in) > DateTime.Now.AddSeconds(-10))
                return;
            string user = Session.Develop ? Session.getParam("TAMRO_TEST", "USER") : Session.getParam("TAMRO", "USER");
            string password = Session.Develop ? Session.getParam("TAMRO_TEST", "PASSWORD") : Session.getParam("TAMRO", "PASSWORD");

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
                httpClient.BaseAddress = baseAddress;
                httpClient.SetBearerToken(accessToken.access_token);
                result = await func(httpClient);
            }
            return result;
        }

        public async Task<T> PutAsync<T>(string resource, object content)
        {
            return await Execute(async (httpClient) =>
            {
                using (var response = await httpClient.PutAsJsonAsync(resource, content))
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
        public async Task<T> PostAsync<T>(string resource, object content)
        {
            return await Execute(async (httpClient) =>
            {
                using (var response = await httpClient.PostAsJsonAsync(resource, content))
                    return await response.Content.ReadAsAsync<T>();
            });
        }
    }
}
