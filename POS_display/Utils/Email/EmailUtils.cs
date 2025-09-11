using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace POS_display.Utils.Email
{
    public class EmailUtils : IEmailUtils
    {
        private readonly SmtpClient _smtpClient = null;
        private readonly string _senderEmail = string.Empty;

        public EmailUtils(string host, int port, string username, string password)
        {
            _smtpClient = new SmtpClient
            {
                Port = port,
                Host = host,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password),
                DeliveryMethod = SmtpDeliveryMethod.Network
                
            };
            _senderEmail = Session.getParam("EMAIL", "SENDEREMAIL") ?? throw new ArgumentNullException();
        }

        public async Task SendEmail(string subject, string body, string toEmail, string replayTo = "", List<Attachment> attachments = null)
        {
            try
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(_senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                if (!string.IsNullOrEmpty(replayTo))
                    message.ReplyToList.Add(new MailAddress(replayTo));

                if (attachments?.Count > 0) 
                {
                    foreach(var attach in attachments)
                        message.Attachments.Add(attach);
                }

                message.To.Add(new MailAddress(toEmail));
                await _smtpClient.SendMailAsync(message);
                Serilogger.GetLogger().Information($"Email has been sent to: {toEmail}; Subject: {subject}; Body: {body}");
            }
            catch (Exception ex) 
            {
                Serilogger.GetLogger().Error(ex, $"Email sending to: {toEmail} failed. Error: {ex.Message}");
            }
        }

    }
}
