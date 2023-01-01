using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkOffice.Services.Email
{
    public class EmailMessage
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public Dictionary<string, string> Content { get; set; }
        public string TemplateName { get; set; }
        public IFormFileCollection Attachments { get; set; }
        public string Sender { get; set; }

        public EmailMessage(IEnumerable<string> to, string subject, Dictionary<string, string> content, string templateName, string sender, IFormFileCollection attachments)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Content = content;
            TemplateName = templateName;
            Attachments = attachments;
            Sender = sender;
        }
    }
}
