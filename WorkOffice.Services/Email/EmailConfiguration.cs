using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Services
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SenderEmail { get; set; }
        public string  AdminEmail { get; set; }
        public string MailJetApiKey { get; set; }
        public string MailJetApiSecret { get; set; }
    }
}
