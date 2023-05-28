using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
   public class HospitalSeeder
    {
        private readonly DataContext _context;
        public HospitalSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new Hospital { HospitalId = 1, Code = "HOS-1", Name = "HOSPITAL- 1",  ClientId = 1 });
            AddNewData(new Hospital { HospitalId = 2, Code = "HOS-2", Name = "HOSPITAL- 2",  ClientId = 1 });
            AddNewData(new Hospital { HospitalId = 3, Code = "HOS-3", Name = "HOSPITAL- 3",  ClientId = 1 });
            AddNewData(new Hospital { HospitalId = 4, Code = "HOS-4", Name = "HOSPITAL- 4",  ClientId = 1 });
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(Hospital data)
        {
            var existingData = _context.Hospitals.FirstOrDefault(p => p.Name.ToLower().Trim() == data.Name.ToLower().Trim());
            if (existingData == null)
            {
                _context.Hospitals.Add(data);
            }
        }
    }
}
