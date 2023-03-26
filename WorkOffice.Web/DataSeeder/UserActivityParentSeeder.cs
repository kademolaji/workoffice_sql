using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Common;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
    public class UserActivityParentSeeder
    {
        private readonly DataContext _context;
        public UserActivityParentSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new UserActivityParent { UserActivityParentId  = (long)UserActivityParentEnum.Account, UserActivityParentName = Enum.GetName(typeof(UserActivityParentEnum), UserActivityParentEnum.Account).Replace("_", " "), ClientId = 1 });
            AddNewData(new UserActivityParent { UserActivityParentId = (long)UserActivityParentEnum.Organization, UserActivityParentName = Enum.GetName(typeof(UserActivityParentEnum), UserActivityParentEnum.Organization).Replace("_", " "), ClientId = 1 });
          
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(UserActivityParent data)
        {
            var existingData = _context.UserActivityParents.FirstOrDefault(p => p.UserActivityParentName == data.UserActivityParentName);
            if (existingData == null)
            {
                _context.UserActivityParents.Add(data);
            }
        }
    }
}
