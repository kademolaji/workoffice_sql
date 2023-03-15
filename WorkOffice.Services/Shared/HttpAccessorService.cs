using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkOffice.Contracts.ServicesContracts;

namespace WorkOffice.Services
{
    public class HttpAccessorService : IHttpAccessorService
    {
        private readonly IHttpContextAccessor _httpAccessor;
        public HttpAccessorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpAccessor = httpContextAccessor;
        }

        public Guid GetCurrentClientId()
        {
            var sub = _httpAccessor.HttpContext.User.Claims
               .FirstOrDefault(x => x.Type == "ClientId");
            if (sub != null)
            {
                return Guid.Parse(sub.Value);
            }
            return Guid.Parse("74B638B6-0A8D-4955-BDAE-74F5E485E5F2");
        }
        public Guid GetCurrentUserId()
        {
            var sub = _httpAccessor.HttpContext.User.Claims
             .FirstOrDefault(x => x.Type == "UserId");
            if (sub != null)
            {
                return Guid.Parse(sub.Value);
            }
            return default(Guid);
        }
        public string GetCurrentUserName()
        {
            var sub = _httpAccessor.HttpContext.User.Claims
               .FirstOrDefault(x => x.Type == "UserName");
            if (sub != null)
            {
                return sub.Value;
            }
            return "AllowedAnonymous";
        }

        public string GetCurrentUserEmail()
        {
            var sub = _httpAccessor.HttpContext.User.Claims
               .FirstOrDefault(x => x.Type == "Email");
            if (sub != null)
            {
                return sub.Value;
            }
            return "";
        }

        public String GetClientIP()
        {
            return _httpAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public String GetHostAddress()
        {
            return _httpAccessor.HttpContext.Request.Host.Host.ToString();
        }
    }
}

