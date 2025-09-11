using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace POS_display.Utils.Email
{
    public interface IEmailUtils
    {
        Task SendEmail(string subject, string body, string toEmail, string replayTo = "", List<Attachment> attachments = null);
    }
}
