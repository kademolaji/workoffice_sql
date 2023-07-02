using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
   public class WaitingTypeSeeder
    {
        private readonly DataContext _context;
        public WaitingTypeSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new WaitingType { WaitingTypeId = 1, Code = "Active", Name = "Outpatient Waiting List",  ClientId = 1 });
            AddNewData(new WaitingType { WaitingTypeId = 2, Code = "Planned", Name = "Inpatient Waiting List",  ClientId = 1 });
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(WaitingType data)
        {
            var existingData = _context.WaitingTypes.FirstOrDefault(p => p.Name.ToLower().Trim() == data.Name.ToLower().Trim());
            if (existingData == null)
            {
                _context.WaitingTypes.Add(data);
            }
        }
    }
}
