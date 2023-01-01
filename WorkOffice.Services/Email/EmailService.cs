using MailKit.Net.Smtp;
using MimeKit;
using WorkOffice.Services.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace WorkOffice.Services.Shared
{
    public interface IEmailService
    {
        void SendEmail(EmailMessage message);
        Task SendEmailAsync(EmailMessage message);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IHostingEnvironment _env;
        public EmailService(EmailConfiguration emailConfig, IHostingEnvironment env)
        {
            _emailConfig = emailConfig;
            _env = env;
        }

        public void SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var mailMessage = CreateEmailMessage(message);
            await SendAsync(mailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("WorkOffice", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = PopulateBody(message.Content, message.TemplateName) };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
        private string PopulateBody(Dictionary<string, string> Content, string templateName)
        {
            var pathToFile = _env.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplates"
                           + Path.DirectorySeparatorChar.ToString()
                           + templateName;
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(pathToFile))
            {
                body = reader.ReadToEnd();
            }
            foreach (KeyValuePair<string, string> kvp in Content)
                body = body.Replace(kvp.Key, kvp.Value);

            return body;
        }
    }
}