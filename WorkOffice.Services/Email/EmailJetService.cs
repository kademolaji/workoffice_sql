using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using WorkOffice.Domain.Entities;
using System.Threading.Tasks;

namespace WorkOffice.Services
{
    public interface IEmailJetService
    {
        Task VerificationEmail(UserAccount account, string password, string verifyUrl);
        Task ResetPasswordEmail(UserAccount account, string resetUrl);
        Task VolunterVerifiedEmail(UserAccount account, string loginUrl);
       
    }
    public class EmailJetService : IEmailJetService
    {
        private readonly MailjetClient _client;
        private readonly EmailConfiguration _emailConfig;
        public EmailJetService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
            _client = new MailjetClient(_emailConfig.MailJetApiKey, _emailConfig.MailJetApiSecret)
            {
                Version = ApiVersion.V3_1
            };

        }

        public async Task VerificationEmail(UserAccount account, string password, string verifyUrl)
        {
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
                .Property(Send.Messages, new JArray {
                 new JObject {
                 {"From", new JObject {
                  {"Email", "info@skilledally.org"},
                  {"Name", "RTT Trainer"}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email", account.Email},
                   {"Name", $"{account.FirstName} {account.LastName}"}
                   }
                  }},
                 {"TemplateID", 4580213},
                 {"TemplateLanguage", true},
                 {"Subject", "RTT Account Verification"},
                 {"Variables", new JObject {
                  {"firstName", account.FirstName},
                  {"password", password},
                  {"verifyurl", verifyUrl}    
                  }}
                 }
                    });
            MailjetResponse response = await _client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
              
            }
            else
            {
          
            }

        }

        public async Task ResetPasswordEmail(UserAccount account, string resetUrl)
        {
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
                .Property(Send.Messages, new JArray {
                 new JObject {
                 {"From", new JObject {
                  {"Email", "info@skilledally.org"},
                  {"Name", "RTT Trainer"}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email", account.Email},
                   {"Name", $"{account.FirstName} {account.LastName}"}
                   }
                  }},
                 {"TemplateID", 4578642},
                 {"TemplateLanguage", true},
                 {"Subject", "Password Reset Request "},
                 {"Variables", new JObject {
                  {"firstname", account.FirstName},
                  {"reset_password_link", resetUrl}
                  }}
                 }
                    });
            MailjetResponse response = await _client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
            
            }
            else
            {
             
            }

        }

        public async Task VolunterVerifiedEmail(UserAccount account, string loginUrl)
        {
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
                .Property(Send.Messages, new JArray {
                 new JObject {
                 {"From", new JObject {
                  {"Email", "info@skilledally.org"},
                  {"Name", "RTT Trainer"}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email", account.Email},
                   {"Name", $"{account.FirstName} {account.LastName}"}
                   }
                  }},
                 {"TemplateID", 4580228},
                 {"TemplateLanguage", true},
                 {"Subject", "Account Verified. Welcome to RTT Trainer"},
                 {"Variables", new JObject {
                  {"firstName", account.FirstName},
                  {"login_url", loginUrl}
                  }}
                 }
                    });
            MailjetResponse response = await _client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
           
            }
            else
            {
               
            }

        }

    
    }
}
