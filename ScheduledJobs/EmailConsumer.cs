using MassTransit;
using Messages.Shared;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledJobs
{
    public class EmailConsumer : IConsumer<SendEmail>
    {
        readonly ILogger<EmailConsumer> _logger;

        public EmailConsumer(ILogger<EmailConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SendEmail> context)
        {
            SmtpClient smtpClient = new SmtpClient("kampanyan.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential("iletisim@kampanyan.com", "sanane");


            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(context.Message.From);
            mail.To.Add(context.Message.To);
            mail.Subject = context.Message.Subject;
            mail.Body = context.Message.Body;
            mail.IsBodyHtml = true;


            smtpClient.Send(mail);

            return Task.CompletedTask;
        }
    }
}
