using System.Net.Mail;

namespace Messages.Shared
{
    public class SendEmail
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string From { get; set; }
    }
}