using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IHttpAccessorService
    {
        long GetCurrentClientId();
        long GetCurrentUserId();
        string GetCurrentUserName();
        string GetCurrentUserEmail();
        String GetClientIP();
        String GetHostAddress();
    }
}

