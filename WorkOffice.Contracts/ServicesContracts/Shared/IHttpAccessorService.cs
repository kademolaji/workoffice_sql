using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.ServicesContracts.Shared
{
    public interface IHttpAccessorService
    {
        string GetCurrentUserId();
        string GetCurrentUserName();
        string GetCurrentUserEmail();
        String GetClientIP();
        String GetHostAddress();
    }
}

