using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkOffice.Common.Enums;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Services
{
   public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly DataContext _context;


        public UserAuthorizationService(DataContext context)
        {
            this._context = context;
        }
        public bool CanPerformActionOnResource(long userId, long activityId, long clientId, UserActions action)
        {
            IList<UserActionModel> activities = this.loadUserResources(userId, clientId);

            switch (action)
            {
                case UserActions.Approve:
                    return activities.Any(x => x.ActivityId == activityId && x.CanApprove == true);
                case UserActions.Delete:
                    return activities.Any(x => x.ActivityId == activityId && x.CanDelete == true);
                case UserActions.Update:
                    return activities.Any(x => x.ActivityId == activityId && x.CanEdit == true);
                case UserActions.View:
                    return activities.Any(x => x.ActivityId == activityId && x.CanView == true);
                case UserActions.Add:
                    return activities.Any(x => x.ActivityId == activityId && x.CanAdd == true);
                default:
                    return false;
            }
        }


        private IList<UserActionModel> loadUserResources(long userId, long clientId)
        {
            var userRoles = (from a in _context.UserAccountRoles
                              join b in _context.UserRoleDefinitions on a.UserRoleDefinitionId equals b.UserRoleDefinitionId
                              where a.UserAccountId == userId && a.ClientId == clientId
                              select a.UserRoleDefinitionId).ToList();

            IList<UserActionModel> userActivities = null;
            IList<UserActionModel> additionalActivities = null;
            IList<UserActionModel> fullActivities = null;

            if (userRoles.Any())
            {
                userActivities = _context.UserRoleActivities.Where(x => userRoles.Contains(x.UserRoleDefinitionId))
                                        .Select(x => new UserActionModel
                                        {
                                            ActivityId = x.UserActivityId,
                                            CanAdd = (bool)x.CanAdd,
                                            CanApprove = (bool)x.CanApprove,
                                            CanDelete = (bool)x.CanDelete,
                                            CanEdit = (bool)x.CanEdit,
                                            CanView = (bool)x.CanView

                                        }).ToList();


                additionalActivities = _context.UserAccountAdditionalActivities.Where(x => x.UserAccountId == userId)
                                        .Select(x => new UserActionModel
                                        {
                                            ActivityId = x.UserActivityId,
                                            CanAdd = (bool)x.CanAdd,
                                            CanApprove = (bool)x.CanApprove,
                                            CanDelete = (bool)x.CanDelete,
                                            CanEdit = (bool)x.CanEdit,
                                            CanView = (bool)x.CanView

                                        }).ToList();

                if (userActivities.Any())
                {
                    fullActivities = userActivities.Concat(additionalActivities).ToList();
                }

            }
            return fullActivities;
        }
    }
}
