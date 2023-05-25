using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
    public class UserRoleDefinitionSeeder
    {
        private readonly DataContext _context;
        public UserRoleDefinitionSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new UserRoleDefinition { RoleName ="SuperAdmin", UserRoleActivity = GetUserRoleActivity(), ClientId = 1 });
          
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(UserRoleDefinition data)
        {
            var existingData = _context.UserRoleDefinitions.FirstOrDefault(p => p.RoleName == data.RoleName);
            if (existingData == null)
            {
                _context.UserRoleDefinitions.Add(data);
            }
        }

        private List<UserRoleActivity> GetUserRoleActivity()
        {
            List<UserRoleActivity> activities = new List<UserRoleActivity>();
            var userActivities = _context.UserActivities.ToList();

            if (userActivities.Count > 0)
            {
                foreach (var item in userActivities)
                {
                    var newActivity = new UserRoleActivity()
                    {
                        UserActivityId = item.UserActivityId,
                        CanAdd = true,
                        CanEdit = true,
                        CanApprove = true,
                        CanDelete = true,
                        CanView = true,
                        ClientId = 1
                    };
                    activities.Add(newActivity);
                }
            }
            return activities;
        }
    }
}

