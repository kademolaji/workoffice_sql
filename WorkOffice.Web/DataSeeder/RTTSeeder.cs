using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
   public class RTTSeeder
    {
        private readonly DataContext _context;
        public RTTSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new RTT { RTTId = 1, Code = "C", Name = "CLOSED", ClientId = 1 });
            AddNewData(new RTT { RTTId = 2, Code = "C-T", Name = "CLOCK-TICKING", ClientId = 1 });
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(RTT data)
        {
            var existingData = _context.RTTs.FirstOrDefault(p => p.Name.ToLower().Trim() == data.Name.ToLower().Trim());
            if (existingData == null)
            {
                _context.RTTs.Add(data);
            }
        }
    }
}
