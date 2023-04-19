using System;
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
            
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Activity, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Activity).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Setup, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.AppType, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.AppType).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Setup, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Consultant, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Consultant).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Setup, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Hospital, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Hospital).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Setup, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Pathway_Status, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Pathway_Status).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Setup, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.RTT, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.RTT).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Setup, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Specialty, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Specialty).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Setup, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Waiting_Type, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Waiting_Type).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Setup, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Ward, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Ward).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.Setup, ClientId = 1 });

            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Patient_Information, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Patient_Information).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.NHS, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Add_Appointment, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Add_Appointment).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.NHS, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Patient_Document, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Patient_Document).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.NHS, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Add_Waitinglist, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Add_Waitinglist).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.NHS, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.View_Waitinglist, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.View_Waitinglist).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.NHS, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Add_Pathway, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Add_Pathway).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.NHS, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Validate_now, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Validate_now).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.NHS, ClientId = 1 });

            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Patient_Appointment, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Patient_Appointment).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.View_Appointment, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.View_Booked_Appointment, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.View_Booked_Appointment).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.View_Appointment, ClientId = 1 });
            AddNewData(new UserActivity { UserActivityId = (long)UserActivitiesEnum.Cancelled_Appointment, UserActivityName = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Cancelled_Appointment).Replace("_", " "), UserActivityParentId = (long)UserActivityParentEnum.View_Appointment, ClientId = 1 });

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

