﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Common;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{

    public class UserActivitiesSeeder
    {
        private readonly DataContext _context;
        public UserActivitiesSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.User_Accounts, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.User_Accounts).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Account, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.User_Roles, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.User_Roles).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Account, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Locations, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Locations).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Organization, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Company_Structures, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Company_Structures).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Organization, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Structure_Definition, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Structure_Definition).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Organization, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Custom_Identity_Settings, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Custom_Identity_Settings).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Organization, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.General_Information, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.General_Information).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Organization, ClientId = 1 });

            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(UserActivity data)
        {
            var existingData = _context.UserActivities.FirstOrDefault(p => p.UserActivityName == data.UserActivityName);
            if (existingData == null)
            {
                _context.UserActivities.Add(data);
            }
        }
    }
}

