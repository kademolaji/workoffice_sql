using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.ServicesContracts.Shared
{
    public interface IHttpAccessorService
    {
        long GetCurrentUserId();
        string GetCurrentUserName();
        string GetCurrentUserEmail();
        String GetClientIP();
        String GetHostAddress();
    }
}

