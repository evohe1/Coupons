using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace SiteDeals.MVCWebUI.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }



        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage message = new MailMessage("iletisim@kampanyan.com", email);
            message.Subject = subject;
            message.Body = htmlMessage;
         
            // Credentials are necessary if the server requires the client
            // to authenticate before it will send email on the client's behalf.
   

            using (SmtpClient client = new SmtpClient("kampanyan.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("iletisim@kampanyan.com", "sanane");
                try
                {
                    await client.SendMailAsync(message).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SendEmailAsync");
                    throw ex;
                }
            }

         
        }
    }
}
