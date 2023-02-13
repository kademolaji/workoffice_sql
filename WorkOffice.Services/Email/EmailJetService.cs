using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using WorkOffice.Domain.Entities;
using WorkOffice.Services.Email;
using System.Threading.Tasks;

namespace WorkOffice.Services.Email
{
    public interface IEmailJetService
    {
        Task VerificationEmail(Domain.Entities.Account.User account, string verifyUrl);
        Task ResetPasswordEmail(Domain.Entities.Account.User account, string resetUrl);
        Task VolunterVerifiedEmail(Domain.Entities.Account.User account, string loginUrl);
       
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

        public async Task VerificationEmail(Domain.Entities.Account.User account, string verifyUrl)
        {
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
                .Property(Send.Messages, new JArray {
                 new JObject {
                 {"From", new JObject {
                  {"Email", "info@skilledally.org"},
                  {"Name", "SkilledAlly"}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email", account.Email},
                   {"Name", $"{account.FirstName} {account.LastName}"}
                   }
                  }},
                 {"TemplateID", 4580213},
                 {"TemplateLanguage", true},
                 {"Subject", "SkilledAlly Account Verification"},
                 {"Variables", new JObject {
                  {"firstName", account.FirstName},
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

        public async Task ResetPasswordEmail(Domain.Entities.Account.User account, string resetUrl)
        {
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
                .Property(Send.Messages, new JArray {
                 new JObject {
                 {"From", new JObject {
                  {"Email", "info@skilledally.org"},
                  {"Name", "Skilledally"}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email", "kademolaji@yahoo.com"},
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

        public async Task VolunterVerifiedEmail(Domain.Entities.Account.User account, string loginUrl)
        {
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
                .Property(Send.Messages, new JArray {
                 new JObject {
                 {"From", new JObject {
                  {"Email", "info@skilledally.org"},
                  {"Name", "SkilledAlly"}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email", account.Email},
                   {"Name", $"{account.FirstName} {account.LastName}"}
                   }
                  }},
                 {"TemplateID", 4580228},
                 {"TemplateLanguage", true},
                 {"Subject", "Account Verified. Welcome to SkilledAlly"},
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
