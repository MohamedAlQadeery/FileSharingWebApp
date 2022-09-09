using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;

namespace FileSharingWeb.Helpers
{
    public class EmailMessage
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailMessage(IEnumerable<string> to, string subject, string body)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(t => new MailboxAddress("email", t)));
            Subject = subject;
            Body = body;
        }
    }
}