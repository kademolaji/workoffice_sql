using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
   public class WardSeeder
    {
        private readonly DataContext _context;
        public WardSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new Ward { WardId = 1, Code = "A1", Name = "FEMALE  WARD",  ClientId = 1 });
            AddNewData(new Ward { WardId = 2, Code = "A2", Name = "MALE WARD",  ClientId = 1 });
            AddNewData(new Ward { WardId = 3, Code = "A3", Name = "CHILDREEN WARD", ClientId = 1 });
            AddNewData(new Ward { WardId = 4, Code = "A4", Name = "GENERAL WARD", ClientId = 1 });
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(Ward data)
        {
            var existingData = _context.Wards.FirstOrDefault(p => p.Name.ToLower().Trim() == data.Name.ToLower().Trim());
            if (existingData == null)
            {
                _context.Wards.Add(data);
            }
        }
    }
}
