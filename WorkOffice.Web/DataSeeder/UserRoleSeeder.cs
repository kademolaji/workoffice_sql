using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
    public class UserRoleSeeder
    {
        private readonly DataContext _context;
        public UserRoleSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new UserAccountRole { UserAccountId =1, UserRoleDefinitionId=1,  ClientId = 1 });
            
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(UserAccountRole data)
        {
            var existingData = _context.UserAccountRoles.FirstOrDefault(p => p.UserAccountRoleId == 1);
            if (existingData == null)
            {
                _context.UserAccountRoles.Add(data);
            }
        }
    }
}

