using Microsoft.AspNet.SignalR.Client;
using POS_display.Models.FeedbackTerminal;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static POS_display.Enumerator;

namespace POS_display.Utils
{
    public class FeedbackTerminal : IDisposable
    {
        private HubConnection HubConnection { get; set; }
        private IHubProxy FeedbackTerminalHub { get; set; }
        public delegate void SignatureSubmitEventHandler(string signatureData);
        public event SignatureSubmitEventHandler SignatureSubmitEvent;
        public async Task Connect()
        {
            if (HubConnection?.State == ConnectionState.Connected)
                return;
            HubConnection?.TraceWriter?.Close();
            Session.FeedbackTerminalMapping = await GetAsync<FeedbackTerminalMapping>($"information?ComputerIP={Session.LocalIP}");
            if (string.IsNullOrWhiteSpace(Session.FeedbackTerminalMapping?.TerminalIP))
                return;
            HubConnection = new HubConnection(Session.FeedbackTerminalAPI);
            HubConnection.Headers.Add("clientId", Session.FeedbackTerminalMapping.PharmacyCode);
            HubConnection.Headers.Add("numberOfCashDesk", Session.FeedbackTerminalMapping.NumberOfCashDesk);
            FeedbackTerminalHub = HubConnection.CreateHubProxy("FeedbackTerminalHub");
            FeedbackTerminalHub.On("createAccount", (Action<string>)(id =>
            {
                System.Diagnostics.Process.Start($"{Session.FeedbackTerminalAPI}/Customer/Index/{id}");
            }));
            FeedbackTerminalHub.On("SignatureResponse", (Action<Transaction>)(data =>
            {
                SignatureSubmitEvent?.Invoke(data?.CustomerData?.SignatureBase64);
            }));

            await HubConnection.Start();
        }

        #region Requests
        public void CreateAccount(CRMLoyaltyCardType cardType = CRMLoyaltyCardType.None)
        { 
            string URL = $"{Session.FeedbackTerminalAPI}/Customer/Index?" +
                $"feedbackTerminalAddress={Session.FeedbackTerminalMapping.FeedbackTerminalAddress}&" +
                $"systemId={Session.FeedbackTerminalMapping.PharmacyCode}&" +
                $"numberOfCashDesk={Session.FeedbackTerminalMapping.NumberOfCashDesk}&" +
                $"forceTransaction=false&" +
                $"employee={Session.User.DisplayName}&";

            if (cardType != CRMLoyaltyCardType.None) 
            {
                URL += $"cardtype={(int)cardType}";
            }      

            System.Diagnostics.Process.Start(URL);

            Session.FeedbackTerminal.ExecuteAction<BaseRequest>(RequestName.CustomerCancel);
        }

        public void Dispose()
        {
            if (string.IsNullOrWhiteSpace(Session.FeedbackTerminalMapping?.FeedbackTerminalAddress))
                return;
            if(HubConnection != null) HubConnection.Dispose();
        }

        public void ExecuteAction<T>(Models.FeedbackTerminal.RequestName requestName, T request = null) where T : Models.FeedbackTerminal.BaseRequest
        {
            Program.Display1.ExecuteAsyncAction(async () =>
            {
                if (string.IsNullOrWhiteSpace(Session.FeedbackTerminalMapping.FeedbackTerminalAddress))
                    return;
                await Connect();
                if (request == null)
                    request = Activator.CreateInstance<T>();
                request.FeedbackTerminalAddress = Session.FeedbackTerminalMapping.FeedbackTerminalAddress;
                request.SystemId = Session.FeedbackTerminalMapping.PharmacyCode;
                request.NumberOfCashDesk = Session.FeedbackTerminalMapping.NumberOfCashDesk;
                request.Employee = Session.User.DisplayName;
                await FeedbackTerminalHub?.Invoke(requestName.ToString(), request);
            });
        }

        private async Task<T> GetAsync<T>(string url)
        {
            T result;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Session.FeedbackTerminalAPI);
                using (var response = await httpClient.GetAsync($"/api/{url}"))
                    result = await response.Content.ReadAsAsync<T>();
            }

            return result;
        }
        #endregion
    }
}
