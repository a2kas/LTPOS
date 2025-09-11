using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Utils
{
    public class TamroIntranetGateway
    {
        private readonly Uri baseAddress;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string userName;
        private readonly string password;

        internal Items.TamroIntranetGatewayAccessToken accessToken { get; set; }
        internal readonly string url;
        public TamroIntranetGateway()
        {
                var session = Session.Develop ? "INTRANET_TEST" : "INTRANET";
                baseAddress = new Uri(Session.getParam(session, "URL_BACKEND"));
                userName = Session.getParam(session, "USERNAME");
                password = Session.getParam(session, "PASSWORD");
                url = Session.getParam(session, "URL");
                clientId = Session.getParam(session, "CLIENTID");
                clientSecret = Session.getParam(session, "CLIENTSECRET");
        }

        public async Task GetToken()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddress;

                var tokenContent = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password)
                };

                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                using (var response = await client.PostAsync("/connect/token", new FormUrlEncodedContent(tokenContent)))
                {
                    accessToken = await response.Content.ReadAsAsync<Items.TamroIntranetGatewayAccessToken>();
                }
            }
        }
    }
}
