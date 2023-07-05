using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Common;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
    public class CustomSettingsSeeder
    {

        private readonly DataContext _context;
        public CustomSettingsSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new CustomIdentityFormatSetting { Prefix = "SSS", Suffix = "N", Activity = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Add_PatientValidation).Replace("_", " "), Digits = 6, LastDigit = 0, Separator = "-", ClientId = 1 });
            AddNewData(new CustomIdentityFormatSetting { Prefix = "GGG", Suffix = "G", Activity = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Patient_Information).Replace("_", " "), Digits = 6, LastDigit = 0, Separator = "-", ClientId = 1 });
            AddNewData(new CustomIdentityFormatSetting { Prefix = "HHH", Suffix = "Y", Activity = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Patient_Document).Replace("_", " "), Digits = 6, LastDigit = 0, Separator = "-", ClientId = 1 });

            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(CustomIdentityFormatSetting data)
        {
            var existingData = _context.CustomIdentityFormatSettings.FirstOrDefault(p => p.Activity == data.Activity);
            if (existingData == null)
            {
                _context.CustomIdentityFormatSettings.Add(data);
            }
        }
    }
}
