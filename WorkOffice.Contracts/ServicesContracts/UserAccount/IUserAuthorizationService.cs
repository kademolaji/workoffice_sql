using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Common.Enums;

namespace WorkOffice.Contracts.ServicesContracts
{
   public interface IUserAuthorizationService
    {
        bool CanPerformActionOnResource(Guid userId, Guid activityId, Guid clientId, UserActions action);
    }
}
