using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkOffice.Contracts.ServicesContracts.Shared;

namespace WorkOffice.Services.Shared
{
    public class HttpAccessorService : IHttpAccessorService
    {
        private readonly IHttpContextAccessor _httpAccessor;
        public HttpAccessorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpAccessor = httpContextAccessor;
        }


        public string GetCurrentUserId()
        {
            var sub = _httpAccessor.HttpContext.User.Claims
               .FirstOrDefault(x => x.Type == "UserId");

            return sub.ToString();
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

