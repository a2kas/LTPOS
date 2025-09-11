using ExternalServices.Gjensidige;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using POS_display.Utils.Email;
using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS_display.Utils.Insurance.Gjensidige
{
    public class GjensidigeService
    {
        #region Members
        private readonly HttpClient _httpClient;
        private readonly string _userName;
        private readonly string _applicationUserName;
        private readonly string _password;
        private readonly string _operationDateTime;
        private const string _emailSubject = "Klaidos pranešimas";
        #endregion

        #region Constructor
        public GjensidigeService(string baseAddress, string userName, string password, string applicationUserName)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress),
                Timeout = TimeSpan.FromMinutes(1)
            };

            _userName = userName;
            _password = password;
            _applicationUserName = applicationUserName;
            _operationDateTime = DateTime.Now.ToString("s");
        }
        #endregion

        #region Public methods
        public async Task<T067ClaimsResponse> CheckItems(T067ClaimsCheckRequest t067ClaimsCheckRequest)
        {
            return await PostRequest<T067ClaimsResponse>("T067Claims/CheckItems", t067ClaimsCheckRequest);
        }

        public async Task<PharmacyClaimResponse> RegisterClaim(T067ClaimsCheckRequest t067ClaimsCheckRequest)
        {
            try
            {
                return await PostRequest<PharmacyClaimResponse>("PharmacyClaim/RegisterClaim", t067ClaimsCheckRequest);
            }
            catch 
            {
                return await PostRequest<PharmacyClaimResponse>("PharmacyClaimGetRegisteredResultRequest", t067ClaimsCheckRequest);
            }
        }

        public async Task<PharmacyClaimCancelCaseResponse> CancelClaims(PharmacyClaimCancelCaseRequest cancelClaimsRequest)
        {
            return await PostRequest<PharmacyClaimCancelCaseResponse>("PharmacyClaim/CancelClaim", cancelClaimsRequest);
        }

        public LoginParameters GetLoginParameters()
        {
            return new LoginParameters()
            {
                HashString = HashString,
                UserName = _userName,
                ApplicationUserName = _applicationUserName,
                OpDateTime = GetOperationDateTime()
            };
        }

        public string GetOperationDateTime()
        {
            return _operationDateTime;
        }

        public string GetHashString()
        {
            return HashString;
        }
        #endregion

        #region Private methods
        private async Task<T> PostRequest<T>(string requestUri, object requestContent) where T : BaseResponse
        {
            var response = default(T);   
            try
            {
                Serilogger.GetLogger("ltpos_data").Information($"Post request to Gjensidige URI : {requestUri} Content: {JsonConvert.SerializeObject(requestContent)}");
                var content = requestContent?.SerializeToUtf8() ?? string.Empty;
                HttpContent httpContent = new StringContent(content, Encoding.UTF8, "text/xml");
                using (var httpResponseMessage = await _httpClient.PostAsync(requestUri, httpContent))
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    Serilogger.GetLogger("ltpos_data").Information($"Response from post: {requestUri} Response: {message}");

                    var parsedString = Regex.Unescape(message);
                    var isoBites = Encoding.GetEncoding("ISO-8859-1").GetBytes(parsedString);
                    var utf8 = Encoding.UTF8.GetString(isoBites, 0, isoBites.Length);
                    response = Tamroutilities.Extensions.Extensions.Deserialize<T>(utf8);
                }

                if (response.Status != 0)
                    Serilogger.GetLogger("ltpos_data").Error($"Request {requestUri} Failed with response: {JsonConvert.SerializeObject(response)}");

                return response;
            }
            catch (Exception ex)
            {
                await SendEmailAboutError(ex.Message, requestContent?.ToJsonString(), response?.ToJsonString());
                Serilogger.GetLogger("ltpos_data").Error($"Request failed: {requestUri}" +
                    $" Error: {JsonConvert.SerializeObject(ex)} " +
                    $"Request: {JsonConvert.SerializeObject(requestContent)}");
                throw new Exception("Užklausa į Gjensidige nesėkminga.");
            }
        }

        private string HashString
        {
            get
            {
                var validationString = _userName + _password + GetOperationDateTime();
                var inputBytes = Encoding.UTF8.GetBytes(validationString);
                var hashedBytes = new SHA256CryptoServiceProvider().ComputeHash(inputBytes);
                return BitConverter.ToString(hashedBytes).Replace("-", "");
            }
        }

        private async Task SendEmailAboutError(string errorMsg, string requestContent, string responseContent)
        {
            var toEmail = Session.getParam("GJN", "NOTIFYEMAIL");
            var emailUtils = Program.ServiceProvider.GetRequiredService<IEmailUtils>();
            var attachments = CreateEmailAttachements(requestContent, responseContent);
            await emailUtils?.SendEmail(
                _emailSubject,
                CreateEmailBody(errorMsg),
                toEmail,
                string.Empty,
                attachments);
            ReleaseAttachmentsMemory(attachments);
        }

        private string CreateEmailBody(string errorMessage) 
        {
            string body = "<b>Informacija:</b>";
            body += $"<br/> Pos ID: {Session.SystemData.prodcustid}_{Session.Devices.debtorname}";
            body += $"<br/> Klaidos pranešimas: {errorMessage}";
            return body;
        }

        private List<Attachment> CreateEmailAttachements(string requestContent, string responseContent)
        {
            List<Attachment> attachments = new List<Attachment>();
            try
            {
                if (!string.IsNullOrWhiteSpace(requestContent))
                {
                    var requestStream = helpers.StringToStream(requestContent);
                    attachments.Add(new Attachment(requestStream, "request.txt", "text/plain"));
                }

                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    var responseStream = helpers.StringToStream(responseContent);
                    attachments.Add(new Attachment(responseStream, "response.txt", "text/plain"));
                }
                return attachments;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger("ltpos_data").Error($"GjensidigeService CreateEmailAttachements method failed", ex);
                return attachments;
            }
        }

        private void ReleaseAttachmentsMemory(List<Attachment> attachments) 
        {
            attachments?.ForEach(a => a.Dispose());
        }
        #endregion

    }
}
