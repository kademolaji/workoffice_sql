using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Common.Enums;

namespace WorkOffice.Contracts.ServicesContracts
{
   public interface IUserAuthorizationService
    {
        bool CanPerformActionOnResource(long userId, long activityId, long clientId, UserActions action);
    }
}
