using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
   public class NHSActivitySeeder
    {
        private readonly DataContext _context;
        public NHSActivitySeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new NHSActivity { NHSActivityId = 1, Code = "OP REG", Name = "OUTPATIENT REGISTARTION", ClientId = 1 });
            AddNewData(new NHSActivity { NHSActivityId = 2, Code = "WL-ACTIVE", Name = "WAITING LIST", ClientId = 1 });
            AddNewData(new NHSActivity { NHSActivityId = 3, Code = "PLANNED-WL", Name = "PLANNED - WAITING LIST", ClientId = 1 });
            AddNewData(new NHSActivity { NHSActivityId = 4, Code = "OP DISC", Name = "OUTPATIENT DISCHARGED", ClientId = 1 });
            AddNewData(new NHSActivity { NHSActivityId = 5, Code = "INCOM DISC", Name = "INCOMPLETE - DISCHARGED", ClientId = 1 });
            AddNewData(new NHSActivity { NHSActivityId = 6, Code = "TCI DATE", Name = "TO COME IN DATE FOR ( SURGERY OR DIAGS)", ClientId = 1 });
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(NHSActivity data)
        {
            var existingData = _context.NHSActivities.FirstOrDefault(p => p.Name.ToLower().Trim() == data.Name.ToLower().Trim());
            if (existingData == null)
            {
                _context.NHSActivities.Add(data);
            }
        }
    }
}
